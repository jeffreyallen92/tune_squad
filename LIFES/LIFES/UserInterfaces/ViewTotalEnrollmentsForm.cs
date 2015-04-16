﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Runtime.InteropServices;

namespace LIFES.UserInterfaces
{
    /*
     * Class Name: ViewTotalEnrollments.cs
     * Author: Riley Smith
     * Date: 4/8/2015
     * Modified by: Riley Smith
     * 
     * Description: This is the driver class for the View Total Enrollments GUI Window.
     * 
     * Initially generated by Visual Studio GUI builder.
     */
    public partial class ViewTotalEnrollmentsForm : Form
    {
        //Constants Used for transition animations
        const int AW_SLIDE = 0X40000;
        const int AW_CENTER = 0x00000010;
        const int AW_HOR_POSITIVE = 0X1;
        const int AW_HOR_NEGATIVE = 0X2;
        const int AW_BLEND = 0X80000;

        [DllImport("user32")]
        static extern bool AnimateWindow(IntPtr hwnd, int time, int flags);

        public ViewTotalEnrollmentsForm()
        {
            InitializeComponent();
        }

        /*
         * Method: OnLoad
         * Parameters: EventArgs e
         * Output: N/A
         * Created By: Riley Smith
         * Date: 3/30/2015
         * Modified By: Riley Smith
         * 
         * Override the function that loads the Form.
         * Animates the window as it opens.
         */
        protected override void OnLoad(EventArgs e)
        {
            AnimateWindow(this.Handle, 200, AW_CENTER | AW_HOR_POSITIVE);
        }
   
    }
}
