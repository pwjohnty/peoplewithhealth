using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PeopleWith
{
    public class moodlist
    {
        public string Text { get; set; }
        public string ImageSource { get; set; }

        public ObservableCollection<moodlist> GetList()
        {
            return new ObservableCollection<moodlist>
        {
            new moodlist { Text = "Angry", ImageSource = "angry.png" },
            new moodlist { Text = "Anxious", ImageSource = "anxious.png" },
            new moodlist { Text = "Bored", ImageSource = "bored.png" },
            new moodlist { Text = "Content", ImageSource = "content.png" },
            new moodlist { Text = "Confused", ImageSource = "confused.png" },
            new moodlist { Text = "Depressed", ImageSource = "depressed.png" },
            new moodlist { Text = "Emotional", ImageSource = "emotional.png" },
            new moodlist { Text = "Excited", ImageSource = "excited.png" },
            new moodlist { Text = "Grateful", ImageSource = "grateful.png" },
            new moodlist { Text = "Happy", ImageSource = "happy.png" },
            new moodlist { Text = "Relaxed", ImageSource = "relaxed.png" },
            new moodlist { Text = "Sad", ImageSource = "sad.png" },
            new moodlist { Text = "Scared", ImageSource = "scared.png" },
            new moodlist { Text = "S**t", ImageSource = "shit.png" },
            new moodlist { Text = "Sick", ImageSource = "sick.png" },
            new moodlist { Text = "Stressed", ImageSource = "stressed.png" },
            new moodlist { Text = "Tired", ImageSource = "tired.png" },           
            new moodlist { Text = "Upset", ImageSource = "upset.png" },          
        };
        }
    }
}
