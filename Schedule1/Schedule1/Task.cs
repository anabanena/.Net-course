using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Schedule1
{
    class Task
    {
        private int id;
        private string task_name;

        public int Id
        {
            get
            {
                return id;
            }

            set
            {
                id = value;
            }
        }

        public string Name
        {
            get
            {
                return task_name;
            }
            set
            {
                task_name = value;
            }
        }
    }
}
