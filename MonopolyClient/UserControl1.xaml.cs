using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace MonopolyClient
{
    /// <summary>
    /// Logique d'interaction pour UserControl1.xaml
    /// </summary>
    public partial class UserControl1 : UserControl
    {
        private const int NumberOfColumns = 10;

        private const int NumberOfFrames = 51;

        private const int FrameWidth = 100;

        private const int FrameHeight = 150;

        public static readonly TimeSpan TimePerFrame = TimeSpan.FromSeconds(0.5);

        private int currentFrame;

        private TimeSpan timeTillNextFrame;

        public UserControl1()
        {
            InitializeComponent();
        }

        private void Rectangle_MouseEnter(object sender, MouseEventArgs e)
        {
            this.timeTillNextFrame += TimeSpan.FromSeconds(1 / 60f);
            if (this.timeTillNextFrame > TimePerFrame)
            {
                this.currentFrame = (this.currentFrame + 1 + NumberOfFrames) % NumberOfFrames;
                var column = this.currentFrame % NumberOfColumns;
                var row = this.currentFrame / NumberOfColumns;

                this.SpriteSheetOffset.X = -column * FrameWidth;
                this.SpriteSheetOffset.Y = -row * FrameHeight;
            }
        }
    }
}
