using Microsoft.AspNetCore.Identity.Data;
using Microsoft.Identity.Client;
using Newtonsoft.Json.Linq;
using System.Text;
using TVManagmentSystem.DbContexti;
using TVManagmentSystem.Models;
using TVManagmentSystem.ResponseRequest;
using TVManagmentSystem.Sources.Enums;

namespace TVManagmentSystem.Services
{
    public class ChanellService : IUserService
    {
        public GlobalDbContext _Db;

        public ChanellService(GlobalDbContext db)
        {
            _Db = db;
        }

        public void LoadChanels()
        {
            var chanells = _Db.Chanelss.ToList();
            _Db.Chanelss.RemoveRange(chanells);//gavasuftavot dzveli chanaweri 
            string currentdir = Directory.GetCurrentDirectory()+"//Arxebi.txt";
            Console.WriteLine( currentdir);
            StreamReader reader = new StreamReader(currentdir);
            if (reader == null) return;
            while (true)
            {
                string shed = reader.ReadLine();
                if (shed != null)
                {
                    if (_Db.Chanelss.Any(io => io.Name.Equals(shed)))
                    {
                        continue;
                    }
                    else
                    {
                        _Db.Chanelss.Add(new Models.Chanell()
                        {
                            Name = shed,
                        });
                        _Db.SaveChanges();
                    }
                }
                else
                {
                    break;
                }

            }
        }

        public void GETAREPORT()
        {
            LoadChanell("http://192.168.20.100/mux/mux_config_en.asp", "100");
            LoadChanell("http://192.168.20.110/mux/mux_config_en.asp", "110");
            LoadChanell("http://192.168.20.120/mux/mux_config_en.asp", "120");
            LoadChanell("http://192.168.20.130/mux/mux_config_en.asp", "130");
            LoadChanell("http://192.168.20.230/mux/mux_config_en.asp", "230");
            LoadChanell("http://192.168.20.200/mux/mux_config_en.asp", "200");
        }

        public List<InfoResponsee> GetInfoByCHanellName(string Name)
        {

            string name = Name.ToLower().Trim();
            var info=_Db.Chanelss.Where(io=>Name.Contains(io.Name)).FirstOrDefault();

            var res = (from inf in _Db._info
                       where inf.ChanellID == info.ID
                       select new InfoResponsee
                       {
                           dateOfRequest = DateTime.Now,
                           Emr_Transcoder = inf.EMR,
                           ChanelName = name,
                           Source = inf.Source
                       }).ToList();
            return res;
        }

        public List<InfoResponsee> GetChanellsByTranscoder(EnumTranscoders choice)
        {
            string emr =((int)choice).ToString();
            var res = (from inf in _Db._info
                       join df in _Db.Chanelss
                       on inf.ChanellID equals df.ID
                       where inf.EMR.Equals(emr)
                       select new InfoResponsee
                       {
                           ChanelName = df.Name,
                           Source = inf.Source,
                           Emr_Transcoder = inf.EMR,
                           dateOfRequest = DateTime.Now
                       }).ToList();

            return res;
        }

        public void DeleteHistory()
        {
            _Db.Chanelss.RemoveRange(_Db.Chanelss.ToList());

            _Db.SaveChanges();
        }

        private  void LoadChanell(string URL,string emrNumber)
        {
            string url = URL ;
            string username = "Admin";
            string password = "sumavisionrd";
            string credentials = Convert.ToBase64String(Encoding.ASCII.GetBytes($"{username}:{password}"));

            using (HttpClient client = new HttpClient())
            {
                client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Basic", credentials);

                try
                {
                    using (HttpResponseMessage response =  client.GetAsync(url).Result)
                    {

                        if (response.IsSuccessStatusCode)
                        {

                            string jsonData = response.Content.ReadAsStringAsync().Result;
                            var axal = jsonData.Split(new string[] { "zOutNode" }, StringSplitOptions.None);
                            List<string> list = new List<string>();
                            for (int i = 1; i < axal.Length - 1; i++)
                            {
                                list.Add(axal[i]);
                            }
                            foreach (var item in list)
                            {
                                var splt = item.Split('{');
                                foreach (var ite in splt)
                                {
                                    if (ite.Contains("name"))
                                    {
                                        ContainsChanell(ite,emrNumber);
                                    }
                                }

                            }


                        }
                        else
                        {
                            Console.WriteLine($"Failed to retrieve data. Status code: {response.StatusCode}");
                        }
                    }
                }
                catch (HttpRequestException e)
                {
                    Console.WriteLine($"Error: {e.Message}");
                }
            }
        }
        public  void ContainsChanell(string name, string emr)
        {

            foreach (var item in _Db.Chanelss.ToList())
            {
                if (name.ToLower().Contains(item.Name.ToLower()))
                {
                    string json = "[ { ";
                    json += name;
                    // json = json.Remove(json.Length - 2, 2);
                    if (json.Contains("node"))
                    {
                        var js = json.Split(new string[] { "nodes" }, StringSplitOptions.None)[0];
                        Console.WriteLine(js);

                        js += " } ]";
                        Console.WriteLine(js);


                        JArray jsonArray = JArray.Parse(js);

                        foreach (JObject jsonObject in jsonArray)
                        {
                            string nam = jsonObject["name"].ToString();
                            string saboloshedegi = name.Split(new string[] { "(SID" }, StringSplitOptions.None)[0];
                            string bolo = saboloshedegi.Split('"')[1];
                            if (_Db._info.Any(io => io.ChanellID == item.ID))
                            {
                                var info = _Db._info.Where(io => io.ChanellID == item.ID).FirstOrDefault();
                                info.Source = bolo;
                                info.EMR = emr;

                            }
                            else
                            {
                                _Db._info.Add(new Info()
                                {
                                    ChanellID = item.ID,
                                    Source = bolo,
                                    EMR = emr,
                                });
                            }
                            _Db.SaveChanges();

                        }
                    }

                }
            }

          
        }
    }
}
