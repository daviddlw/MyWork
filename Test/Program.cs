using System;
using System.IO;
using System.Globalization;
using System.Collections.Generic;
using System.Collections;
using System.Linq;

namespace Test
{
    class Program
    {
        static void Main(string[] args)
        {
            List<TrackInfo> ls = new List<TrackInfo>()
            {
                new TrackInfo(){  SettledTime = new DateTime(2013, 2,1), IsGeneratedKPI=false, Source = SourceOfChannelEnum.Adspace },
                new TrackInfo(){  SettledTime = new DateTime(2013, 3,1), IsGeneratedKPI=false, Source = SourceOfChannelEnum.Custom },
                new TrackInfo(){  SettledTime = new DateTime(2013, 4,1), IsGeneratedKPI=false, Source = SourceOfChannelEnum.PaySearch },
                new TrackInfo(){  SettledTime = new DateTime(2013, 5,1), IsGeneratedKPI=false, Source = SourceOfChannelEnum.Adspace },
                new TrackInfo(){  SettledTime = new DateTime(2013, 6,1), IsGeneratedKPI=true, Source = SourceOfChannelEnum.Search },
                new TrackInfo(){  SettledTime = new DateTime(2013, 8,1), IsGeneratedKPI=false, Source = SourceOfChannelEnum.Search },
                new TrackInfo(){  SettledTime = new DateTime(2013, 7,1), IsGeneratedKPI=false, Source = SourceOfChannelEnum.Custom },
                new TrackInfo(){  SettledTime = new DateTime(2013,10,1), IsGeneratedKPI=true, Source = SourceOfChannelEnum.PaySearch },
                new TrackInfo(){  SettledTime = new DateTime(2013, 9,1), IsGeneratedKPI=false, Source = SourceOfChannelEnum.SiteRecommend }
            };

            List<List<TrackInfo>> lts = DealWithList(ls);

            ShowList(ls);

            Console.ReadKey();
        }
        
        /// <summary>
        /// 处理相应的归因数据
        /// </summary>
        /// <param name="ls"></param>
        /// <returns></returns>
        public static List<List<TrackInfo>> DealWithList(List<TrackInfo> ls)
        {
            ls = ls.OrderBy(n => n.SettledTime).ToList();
            List<int> resultLs = new List<int>();

            int index = 0;
            while (index < ls.Count)
            {
                index = ls.FindIndex(index, n => n.IsGeneratedKPI);
                if (index == -1)
                    break;
                else
                {
                    resultLs.Add(index);
                    index++;
                }
            }

            List<List<TrackInfo>> lts = new List<List<TrackInfo>>();
            int skipIndex = 0;
            for (int i = 0; i < resultLs.Count; i++)
            {
                int tempIndex = resultLs[i] + 1;
                List<TrackInfo> test = new List<TrackInfo>(ls.Skip(skipIndex).Take(tempIndex));
                lts.Add(test);
                skipIndex = tempIndex;
            }

            return lts;
        }

        private void CultureInfoFun()
        {
            CultureInfo ci = new CultureInfo("en-US");
            for (int m = 1; m <= 12; m++)
            {
                int days = ci.Calendar.GetDaysInMonth(DateTime.Now.Year, m);
                for (int i = 1; i <= days; i++)
                {
                    if (new DateTime(DateTime.Now.Year, m, i).DayOfWeek == DayOfWeek.Sunday)
                        Console.WriteLine(new DateTime(DateTime.Now.Year, m, i).ToShortDateString());
                }
            }
        }

        private static void ShowList(List<TrackInfo> ls)
        {
            foreach (var item in ls)
            {
                Console.WriteLine(item);
            }
        }
    }

    class TrackInfo
    {
        public DateTime SettledTime { get; set; }

        public bool IsGeneratedKPI { get; set; }

        public SourceOfChannelEnum Source { get; set; }

        public override string ToString()
        {
            return string.Format("SettledTime:{0}, IsGeneratedKPI:{1}, Source:{2}", SettledTime.ToString("yyyy-MM-dd"), IsGeneratedKPI, Source.ToString());
        }
    }

    enum SourceOfChannelEnum
    {
        Adspace = 1,

        Search,

        PaySearch,

        Direct,

        SiteRecommend,

        Custom
    }
}
