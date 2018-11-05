using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace Monopoly.Model
{
    class PopUp
    {
        public string Message {get; set;}
        public string ActionButtonLabel { get; set;}
        public string SkinPath { get; set; }
        public PopUp(string message, string actionButtonLabel, string skinPath )
        {
            Message = message;
            ActionButtonLabel = actionButtonLabel;
            SkinPath = skinPath;
        }
    }
}
