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

        private static List<string> GetChannelss(string URL)
        {
            List<string> str = new List<string>();
            string url = URL;
            string username = "Admin";
            string password = "sumavisionrd";
            string credentials = Convert.ToBase64String(Encoding.ASCII.GetBytes($"{username}:{password}"));

            using (HttpClient client = new HttpClient())
            {
                client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Basic", credentials);

                try
                {
                    using (HttpResponseMessage response = client.GetAsync(url).Result)
                    {

                        if (response.IsSuccessStatusCode)
                        {

                            Stream jsonData = response.Content.ReadAsStreamAsync().Result;

                            using (StreamReader read = new StreamReader(jsonData))
                            {


                                while (!read.EndOfStream)
                                {
                                    var red = read.ReadLine();
                                    str.Add(red);
                                }

                            }

                        }
                        else
                        {
                            Console.WriteLine($"Failed to retrieve data. Status code: {response.StatusCode}");
                        }
                    }

                    var res = str.SkipWhile(io => !io.Contains("zInNode7")).ToList();

                    var newres = res.TakeWhile(io => !io.Contains("zOutNode1")).ToList();

                    var newfilter = newres.Where(io => io.Contains("SID")).ToList();

                    List<string> lastres = new List<string>();

                    foreach (var item in newfilter)
                    {
                        var re = item.Split(new string[] { "(SID" }, StringSplitOptions.None)[0].Split(new string[] { "{name:\"" }, StringSplitOptions.None)[1].Split('(')[0];
                        lastres.Add(re);
                    }

                    List<string> uniqueWords = lastres.Select(w => w.Trim()).Distinct(StringComparer.OrdinalIgnoreCase).ToList();

                    return uniqueWords;
                    //foreach (var item in uniqueWords)
                    //{
                    //    Console.WriteLine( item);
                    //    Console.WriteLine(  );
                    //}
                    //Console.WriteLine( "end");
                }
                catch (HttpRequestException e)
                {
                    Console.WriteLine($"Error: {e.Message}");
                }
            }
            return null;
        }

        public void LoadChanels()
        {
            List<string> list = new List<string>();
            list.AddRange(GetChannelss("http://192.168.20.160/mux/mux_config_en.asp"));
            list.AddRange(GetChannelss("http://192.168.20.170/mux/mux_config_en.asp"));
            _Db.Chanelss.RemoveRange(_Db.Chanelss.ToList());
            List<string> uniqueWords = list.Select(w => w.Trim()).Distinct(StringComparer.OrdinalIgnoreCase).ToList();
            foreach (var item in uniqueWords)
            {
                _Db.Chanelss.Add(new Chanell()
                {
                    Name = item.ToUpper()
                }) ;

            }
            _Db.SaveChanges();
            Console.WriteLine(uniqueWords.Count);
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
                Console.WriteLine(name);
                if (name.ToUpper().Contains(item.Name.ToUpper()))
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
