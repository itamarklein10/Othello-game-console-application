using System;
using System.Collections.Generic;
using System.Text;

namespace Ex02_Othelo
{
    public class Player
    {
        private string r_Name;
        public char m_Symbol;
        public int m_Points;

        public Player(string i_Name)
        {
            r_Name = i_Name;
            m_Points = 2;
        }

        public char Symbol
        {
            get { return m_Symbol; }
            set { m_Symbol = value; }
        }

        public string Name
        {
            get { return r_Name; }
            set { r_Name = value; }
        }

        public int Points
        {
            get { return m_Points; }
            set { m_Points = value; }
        }
    }
}
