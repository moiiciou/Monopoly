using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace server
{
    class Packet : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        public string Type
        {
            get { return type; }
            set
            {
                type = value;
                OnPropertyChanged("Type");
            }
        }
        public string Content
        {
            get { return content; }
            set
            {
                content = value;
                OnPropertyChanged("Content");
            }
        }

        public string ChatMessage
        {
            get { return chatMessage; }
            set
            {
                chatMessage = value;
                OnPropertyChanged("ChatMessage");
            }
        }

        private string type;
        private string content;
        private string chatMessage;


        protected void OnPropertyChanged(string name)
        {
            PropertyChangedEventHandler handler = PropertyChanged;
            if (handler != null)
            {
                handler(this, new PropertyChangedEventArgs(name));
                Console.WriteLine("New packet received !");
            }
        }

        protected void OnPropertyChanged(PropertyChangedEventArgs e)
        {
            PropertyChanged?.Invoke(this, e);
            Console.WriteLine("New packet received !");
        }

    }
}
