using System;
using System.Collections.Generic;
using System.Text;

namespace LJPmath
{
    public interface IEquationSystem
    {
        void equations(double[] x, double[] f); // equations in the form f(x)=0
        int number(); // number of equations and unknowns
    }
}
