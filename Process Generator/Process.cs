using System;
using System.Collections.Generic;
using System.Text;

namespace Process_Generator
{
    public class Process
    {
        public int num;

        public int r;//intervalul de timp intre momentele la care s-au incarcat in memorie procele i si i + 1  
        public int t; //momentul de timp la care este incarcat procesul i ---  t(i+1) = t(i) + r
        public int life;
        public int TotalProcessingTime = 0;
        public int TotalWaitingTime = 0;
        public int processGetsProcesor = -1;
        public int responseTime = -1;
        //'-1' semnifica faptul ca procesul nu a intrat deloc in coada ready

        public bool terminated = false;
        public int delta = 0; //se retine viata procesului pentru a se putea afisa la final cata viata a avut procesul initial

        public Process(int num, int r, int t, int life)
        {
            this.num = num;
            this.r = r;
            this.t = t;
            this.life = life;
        }

    }
}

