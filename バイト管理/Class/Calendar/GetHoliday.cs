using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Net;
using System.Xml;
using System.Collections;

namespace WorkManage
{
    public class GetHoliday
    {
        /// <summary>
        /// Googleカレンダーから指定した年の祝日を取得する
        /// </summary>
        /// <param name="year">祝日を取得する年（西暦）。</param>
        /// <param name="apiKey">Google API key。</param>
        /// <param name="calendarId">使用するカレンダーのID。</param>
        /// <returns>指定された年の祝日を表す配列。</returns>
        public static NationalHoliday[] GetNationalHolidays(int year, string apiKey, string calendarId, ArrayList LocalHoliday)
        {
            //URLを作成する
            const string googleUrl = "https://www.googleapis.com/calendar/v3/calendars/";
            const string methodString = "events";
            const int maxResults = 100;
            //クエリーを作成する
            string query = string.Format(
                "key={0}&" +
                "timeMin={1}-01-01T00:00:00Z&" +
                "timeMax={2}-01-01T00:00:00Z&" +
                "maxResults={3}",
                apiKey, year, year + 1, maxResults);
            //つなげて完成
            string url = googleUrl + calendarId + "/" + methodString + "?" + query;

            //サーバーからデータを受信する
            HttpWebRequest req = (HttpWebRequest)WebRequest.Create(url);
            HttpWebResponse res = (HttpWebResponse)req.GetResponse();
            Stream st = res.GetResponseStream();
            StreamReader sr = new StreamReader(st);
            //すべてのデータを受信する
            string jsonString = sr.ReadToEnd();
            //後始末
            sr.Close();
            st.Close();
            res.Close();

            //受信したデータを解析する
            System.Web.Script.Serialization.JavaScriptSerializer serializer = new System.Web.Script.Serialization.JavaScriptSerializer();
            Dictionary<string, object> jsonDic = serializer.Deserialize<Dictionary<string, object>>(jsonString);
            
            if (!jsonDic.ContainsKey("items"))
            {
                //itemsがなかったら失敗したと判断する
                return new NationalHoliday[0];
            }

            System.Collections.ArrayList items = (System.Collections.ArrayList)jsonDic["items"];

            //見つかった祝日の名前と日付を取得する
            NationalHoliday[] holidays = new NationalHoliday[items.Count];
            
            int i;

            for (i = 0; i < items.Count; i++)
            {
                Dictionary<string, object> item = (Dictionary<string, object>)items[i];
                string title = (string)item["summary"];
                string startTime = (string)((Dictionary<string, object>)item["start"])["date"];

                if (LocalHoliday.IndexOf(title) == -1) holidays[i] = new NationalHoliday(title, startTime);
            }

            return holidays;
        }

        /// <summary>
        /// Googleカレンダーから指定した年の日本の祝日を取得する
        /// </summary>
        /// <param name="year">祝日を取得する年（西暦）。/param>
        /// <param name="apiKey">使用するカレンダーのID。</param>
        /// <returns>指定された年の祝日を表す配列。</returns>
        public static NationalHoliday[] GetNationalHolidays(int year, string apiKey, ArrayList LocalHoliday)
        {
            //mozilla.org版
            //const string japaneseHolidaysId =
            //    "outid3el0qkcrsuf89fltf7a4qbacgt9@import.calendar.google.com";
            //公式版日本語
            const string japaneseHolidaysId = "ja.japanese%23holiday@group.v.calendar.google.com";
            
            return GetNationalHolidays(year, apiKey, japaneseHolidaysId, LocalHoliday);
        }

        /// <summary>
        /// 祝日を表現したクラス
        /// </summary>
        public class NationalHoliday
        {
            private string _name;

            /// <summary>
            /// 祝日の名前
            /// </summary>
            public string Name
            {
                get { return this._name; }
            }

            private DateTime _date;
            /// <summary>
            /// 祝日の日付
            /// </summary>
            public DateTime Date
            {
                get { return this._date; }
            }

            /// <summary>
            /// NationalHolidayのコンストラクタ
            /// </summary>
            /// <param name="holidayName">祝日の名前</param>
            /// <param name="holidayDate">祝日の日付（RFC3339形式の文字列）</param>
            public NationalHoliday(string holidayName, string holidayDate)
            {
                this._name = holidayName;
                this._date = XmlConvert.ToDateTime(holidayDate, XmlDateTimeSerializationMode.Local);
            }
        }

    }
}
