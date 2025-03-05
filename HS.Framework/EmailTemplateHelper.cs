using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HS.Framework
{
    public class EmailTemplateHelper
    {
        public static string AppointmentType
        {
            get
            {
                return @"<tr>
                    <td style = 'font-weight:bold; vertical-align:top; width:160px;'>
                        Ticket Type <span style = 'float:right; margin-right:5px;'>:</span>
                    </td>
                    <td style = 'vertical-align:top;'> {0}
                    </td>
                    </tr>";
            }
        }
        public static string AssignedTo
        {
            get
            {
                return @"<tr>
                    <td style = 'font-weight:bold; vertical-align:top; width:160px;'>
                        Assigned Tech <span style='float:right; margin-right:5px;'>:</span>
                    </td>
                    <td style='vertical-align:top;'> {0}
                    </td>
                    </tr>";
            }
        }
        public static string AdditionalMember
        {
            get
            {
                return @"<tr>
                    <td style = 'font-weight:bold;vertical-align:top;'>
                        Additional Techs <span style = 'float:right; margin-right:5px;'>:</span>
                    </td>
                    <td style = 'vertical -align:top;' > {0}
                    </td>
                    </tr>";
            }
        }
        public static string ScheduleDate
        {
            get
            {
                return @"<tr>
                    <td style = 'font-weight:bold;vertical-align:top;'>
                        Schedule Date <span style = 'float:right; margin-right:5px;'>:</span>
                    </td>
                    <td style = 'vertical -align:top;'> {0}
                    </td>
                    </tr>";
            }
        }
        public static string ScheduleStartDate
        {
            get
            {
                return @"<tr>
                      <td style = 'font-weight:bold;vertical-align:top;'>
                          Start Time <span style = 'float:right; margin-right:5px;'>:</span>
                      </td>
                      <td style = 'vertical -align:top;'> {0}
                      </td>
                      </tr>";
            }
        }
        public static string AppointmentEndTime
        {
            get
            {
                return @"<tr>
                      <td style = 'font-weight:bold;vertical-align:top;'>
                           End Time <span style = 'float:right; margin-right:5px;'>:</span>
                      </td>
                      <td style = 'vertical -align:top;'> {0}
                      </td>
                      </tr>";
            }
        }
        public static string CustomerName
        {
            get
            {
                return @"<tr>
                    <td style = 'font-weight:bold; vertical-align:top; width:160px;'>
                        Customer Name <span style = 'float:right; margin-right:5px;'>:</span>
                    </td>
                    <td style = 'vertical-align:top;'> {0}
                    </td>
                    </tr>";
            }
        }
        public static string Address
        {
            get
            {
                return @"<tr>
                    <td style = 'font-weight:bold;vertical-align:top;'>
                        Address <span style = 'float:right; margin-right:5px;' >:</span>
                    </td>
                    <td style = 'vertical -align:top;'> {0}
                    </td>
                    </tr>";
            }
        }
        public static string TicketReply
        {
            get
            {
                return @"<tr>
                    <td colspan = '2' style = 'font-weight:bold;vertical-align:top;'>
                        Note by {0}
                    </td>
                    </tr>
                    <tr>
                    <td style = 'font-weight:bold;vertical-align:top;'> {1} 
                    <span style = 'float:right; margin-right:5px;'>:</span>
                    </td>
                    <td style = 'vertical-align:top;'> {2}
                    </td>
                    </tr>";
            }
        }
        public static string TicketNotificationEmailBody
        {
            get
            {
                return @"A notification has been send by {0} for appoinment {1}.
                        <br> <table style = 'width:100%; float:left; border-collapse:collapse; table-layout:fixed;'>
                        {2} {3} {4} {5} {6} {7} {8} {9} {10} </table>";
            }
        }
    }
}
