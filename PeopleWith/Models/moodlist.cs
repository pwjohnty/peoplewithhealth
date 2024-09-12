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
            new moodlist { Text = "Happy", ImageSource = "happy.png" },
            new moodlist { Text = "Sad", ImageSource = "sad.png" },
            new moodlist { Text = "Laughing", ImageSource = "laughing.png" },
            new moodlist { Text = "Sick", ImageSource = "sick.png" },
            new moodlist { Text = "Confused", ImageSource = "confused.png" },
            new moodlist { Text = "Crying", ImageSource = "crying.png" },
            new moodlist { Text = "Angry", ImageSource = "angry.png" },
            new moodlist { Text = "Neutral", ImageSource = "neutral.png" },
            new moodlist { Text = "Tired", ImageSource = "tired.png" },
            new moodlist { Text = "Surprised", ImageSource = "surprised.png" },
            new moodlist { Text = "Loved", ImageSource = "loved.png" },
            new moodlist { Text = "S**t", ImageSource = "shit.png" },
        };
        }
    }
}
