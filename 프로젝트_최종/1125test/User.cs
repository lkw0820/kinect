using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _1125test
{
    public class User
    {
        private string id;
        private int score;

        public string Id
        {
            get { return id; }
            set {
                id = value;
            }
        }
        public int Score
        {
            get { return score; }
            set
            {
                score = value;
            }
        }

    }
}
