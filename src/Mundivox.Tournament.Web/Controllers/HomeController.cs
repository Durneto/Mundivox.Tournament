using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Mundivox.Tournament.Data;
using Mundivox.Tournament.Model;
using Mundivox.Tournament.Web.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace Mundivox.Tournament.Web.Controllers
{
    public class HomeController : Controller
    {
        private readonly DataContext _context;

        public HomeController(DataContext context)
        {
            _context = context;
        }

        public IActionResult Index(IEnumerable<Phase> phaseList = null)
        {
            return View(phaseList);
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        [HttpPost]
        public async Task<IActionResult> TournamentCreate(int selectedKey = 0, int idWinner = 0)
        {
            BracketsViewModel brackets = new BracketsViewModel();

            if (idWinner == 0)
            {
                var phaseList = GetPhaseList();
                var teamList = await _context.Team.ToListAsync();

                if (teamList.Count != 4)
                    throw new Exception("Deve haver exatamente 4 times cadastrados.");


                phaseList.ToList().ForEach(p => brackets.Titles.Add(p.Name.ToString()));
                brackets.Titles.Add("Campeão");

                var phase = phaseList.FirstOrDefault(p => p.Name.Equals("Semifinal"));

                RoundViewModel round = null;
                List<RoundViewModel> roundMatch = new List<RoundViewModel>();
                int key = 1;

                teamList.ToList().ForEach(t =>
                {
                    if (round == null)
                        round = new RoundViewModel { Player1 = new PlayerViewModel { Id = t.Id, Name = t.Name, Onclick = $"nextPhase({key}, 1)" } };
                    else if (round.Player2 == null)
                    {
                        round.Player2 = new PlayerViewModel { Id = t.Id, Name = t.Name, Onclick = $"nextPhase({key}, 2)" };

                        roundMatch.Add((RoundViewModel)round.Clone());
                        round = null;
                        key++;
                    }
                });

                brackets.Rounds.Add(roundMatch);

                //FINAL
                brackets.Rounds.Add(new List<RoundViewModel> { new RoundViewModel { Player1 = new PlayerViewModel { Name = "" }, Player2 = new PlayerViewModel { Name = "" } } });

                //CAMPEÂO
                brackets.Rounds.Add(new List<RoundViewModel> { new RoundViewModel { Player1 = new PlayerViewModel { Name = "" } } });

                HttpContext.Session.SetString("brackets", JsonConvert.SerializeObject(brackets));
            }
            else
            {
                brackets = JsonConvert.DeserializeObject<BracketsViewModel>(HttpContext.Session.GetString("brackets"));

                switch (selectedKey)
                {
                    case 1:
                        if (idWinner == 1)
                        {
                            brackets.Rounds[1][0].Player1 = brackets.Rounds[0][0].Player1;
                            brackets.Rounds[1][0].Player1.Winner = true;
                            brackets.Rounds[1][0].Player1.Onclick = $"nextPhase(3, {brackets.Rounds[0][0].Player1.Id})";
                        }
                        else
                        {
                            brackets.Rounds[1][0].Player1 = brackets.Rounds[0][0].Player2;
                            brackets.Rounds[1][0].Player1.Winner = true;
                            brackets.Rounds[1][0].Player1.Onclick = $"nextPhase(3, {brackets.Rounds[0][0].Player2.Id})";
                        }
                        break;
                    case 2:
                        if (idWinner == 1)
                        {
                            brackets.Rounds[1][0].Player2 = brackets.Rounds[0][1].Player1;
                            brackets.Rounds[1][0].Player2.Winner = true;
                            brackets.Rounds[1][0].Player2.Onclick = $"nextPhase(3, {brackets.Rounds[0][1].Player1.Id})";
                        }
                        else
                        {
                            brackets.Rounds[1][0].Player2 = brackets.Rounds[0][1].Player2;
                            brackets.Rounds[1][0].Player2.Winner = true;
                            brackets.Rounds[1][0].Player2.Onclick = $"nextPhase(3, {brackets.Rounds[0][1].Player2.Id})";
                        }
                        break;
                    case 3:
                        if (brackets.Rounds[1][0].Player1.Id == idWinner)
                            brackets.Rounds[2][0].Player1 = brackets.Rounds[1][0].Player1;
                        else
                            brackets.Rounds[2][0].Player1 = brackets.Rounds[1][0].Player2;
                        break;
                    default:
                        break;
                }
            }

            HttpContext.Session.SetString("brackets", JsonConvert.SerializeObject(brackets));
            return Json(brackets);
        }

        private IEnumerable<Phase> GetPhaseList()
        {
            var list = _context.Phase.ToListAsync().Result;

            if (list.Count == 0)
            {
                _context.Add(new Phase { Name = "Semifinal" });
                _context.Add(new Phase { Name = "Final" });
                _context.SaveChangesAsync().Wait();
            }

            return _context.Phase.ToListAsync().Result;
        }
    }
}
