using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WorkManage
{
    public class CalendarWeek
    {
        private CalendarDay[] _days = null;

        public CalendarWeek(CalendarDay[] days)
        {
            _days = days;
        }

        public CalendarDay[] Days
        {
            get
            {
                return _days;
            }

            set
            {
                _days = value;
            }
        }
    }
}
