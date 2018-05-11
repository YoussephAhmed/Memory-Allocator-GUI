using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace OS_project2
{
    public partial class Form1 : Form
    {

        MEMORY memory = new MEMORY();
        string user_method;
        int ascii_number = 'a';

        public Form1()
        {
            InitializeComponent();
            method.Items.Add("First Fit");
            method.Items.Add("Best Fit");
            method.Items.Add("Worst Fit");
            this.MaximizeBox = false;
            MemoryChart.Titles.Add("Memory Model");
            output_text.ScrollBars = ScrollBars.Both;
            output_text.WordWrap = false;


        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private void label1_Click(object sender, EventArgs e)
        {

        }



        private void show_output_Click(object sender, EventArgs e)
        {



            compact.Visible = true;

            active_processes.Items.Clear();

            for (int i = 0; i < memory.active_memory.Count; i++)
            {
                if (memory.active_memory.ElementAt(i).isHole == false)
                    active_processes.Items.Add(memory.active_memory.ElementAt(i).name);


            }


            for (int i = 0; i < memory.active_memory.Count(); i++)
            {
                output_text.AppendText(" start= ");
                output_text.AppendText(memory.active_memory.ElementAt(i).start.ToString());
                output_text.AppendText(" end= ");
                output_text.AppendText(memory.active_memory.ElementAt(i).end.ToString());
                output_text.AppendText(" name= ");
                output_text.AppendText(memory.active_memory.ElementAt(i).name);
                output_text.AppendText("\n");
            }


            output_text.AppendText("------------------------ \n");



            MemoryChart.Series.Clear();


            MemoryChart.ChartAreas[0].AxisY.Maximum = memory.memorySize;
            MemoryChart.ChartAreas[0].AxisY.Minimum = 0;


            for (int i = 0; i < memory.active_memory.Count; i++)
            {
                MemoryChart.Series.Add(memory.active_memory.ElementAt(i).name);
                MemoryChart.Series[memory.active_memory.ElementAt(i).name].ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.StackedColumn;
                MemoryChart.Series[memory.active_memory.ElementAt(i).name].Points.AddXY("Memory", memory.active_memory.ElementAt(i).size);
                MemoryChart.Series[memory.active_memory.ElementAt(i).name].IsValueShownAsLabel = true;
            }




        }

        private void output_text_TextChanged(object sender, EventArgs e)
        {
            user_method = method.Text;
        }

        private void mem_size_Click(object sender, EventArgs e)
        {


            try
            {
                memory.memorySize = int.Parse(memSize.Text);
                if (memory.memorySize <= 0)
                    throw new System.Exception();
                holes_box.Visible = true;
                memSize.Enabled = false;
                mem_size.Enabled = false;
                memorySizeText.Text = "Size = " + memory.memorySize.ToString();
                memorySizeText.Visible = true;


            }


            catch (Exception)
            {
                MessageBox.Show("Enter a valid memory size value");

            }



            memSize.Clear();

        }


        private void add_hole_Click(object sender, EventArgs e)
        {

            try
            {
                int size = int.Parse(holeSize.Text);
                if (size <= 0)
                    throw new System.Exception();

                int start = int.Parse(holeStart.Text);
                if (start < 0)
                    throw new System.Exception();

                int hallEnd = start + size;
                string name = "hole_ " + memory.holes.Count.ToString();
                memory.add_hole(int.Parse(holeStart.Text), hallEnd, name);
                run.Visible = true;

            }

            catch (Exception)
            {
                MessageBox.Show("Enter valid start and size values");

            }


            holeStart.Clear();
            holeSize.Clear();

        }

        private void run_Click(object sender, EventArgs e)
        {

            memory.fill_memory();
            memory_box.Visible = true;
            run.Enabled = false;
            add_hole.Enabled = false;
            holeSize.Enabled = false;
            holeStart.Enabled = false;
            processes_box.Visible = true;


        }

        private void label4_Click(object sender, EventArgs e)
        {

        }

        private void add_process_Click(object sender, EventArgs e)
        {
            string name = process_name.Text;
            try
            {
                int size = int.Parse(process_size.Text);
                if (size <= 0)
                    throw new System.Exception();
                user_method = method.Text;
                memory.add_process(name, size, user_method);

            }

            catch (Exception)
            {
                MessageBox.Show("Enter valid size value");

            }


            process_name.Clear();
            process_size.Clear();
        }

        private void method_SelectedIndexChanged(object sender, EventArgs e)
        {
            add_process.Visible = true;
        }

        private void deallocate_Click(object sender, EventArgs e)
        {
            string name = active_processes.Text;
            memory.deallocate(name, ascii_number);
            ascii_number++;

            active_processes.Items.Clear();
            for (int i = 0; i < memory.active_memory.Count; i++)
            {
                if (memory.active_memory.ElementAt(i).isHole == false)
                    active_processes.Items.Add(memory.active_memory.ElementAt(i).name);

            }


        }

        private void label2_Click(object sender, EventArgs e)
        {

        }

        private void restart_Click(object sender, EventArgs e)
        {
            Application.Restart();
        }

        private void memSize_TextChanged(object sender, EventArgs e)
        {

        }

        private void holeStart_TextChanged(object sender, EventArgs e)
        {

        }

        private void compact_Click(object sender, EventArgs e)
        {

            MemoryChart.Series.Clear();



            memory.compact(ascii_number);
            ascii_number++;


            compact.Visible = false;
        }
    }


    class MEM_BLOCK : IComparable<MEM_BLOCK>
    {

        public int start;
        public int end;
        public string name;
        public bool isHole;
        public int size;



        public MEM_BLOCK(int start, int end, string name)//constructor
        {
            this.start = start;
            this.end = end;
            this.name = name;
            isHole = true;
            size = end - start;

        }

        public int CompareTo(MEM_BLOCK other)
        {
            if (MEMORY.sortBy == "First Fit")
            {
                if (this.start > other.start) // this 3la el shemal w akbr mn yb2a bertb bl as3'er
                    return 1;

                else if (this.start == other.start)
                {
                    return 0;
                }

                else
                    return -1;
            }

            else if (MEMORY.sortBy == "Best Fit")
            {
                if (this.size > other.size) // this 3la el shemal w akbr mn yb2a bertb bl as3'er
                    return 1;

                else if (this.size == other.size)
                {
                    return 0;
                }

                else
                    return -1;
            }

            else // Worst fit
            {
                if (this.size < other.size) // this 3la el shemal w akbr mn yb2a bertb bl as3'er
                    return 1;

                else if (this.size == other.size)
                {
                    return 0;
                }

                else
                    return -1;
            }




        }
    }







    class MEMORY
    {
        public List<MEM_BLOCK> processes = new List<MEM_BLOCK>();
        public List<MEM_BLOCK> holes = new List<MEM_BLOCK>();
        public List<MEM_BLOCK> filtered_holes = new List<MEM_BLOCK>();
        public List<MEM_BLOCK> compact_memory = new List<MEM_BLOCK>();
        public List<MEM_BLOCK> active_memory = new List<MEM_BLOCK>();
        public int memorySize;
        public static string sortBy;

        public void add_hole(int start, int end, string name)
        {


            MEM_BLOCK hole = new MEM_BLOCK(start, end, name);
            if (end <= memorySize)
                holes.Add(hole);

            else // display error
                MessageBox.Show("The hole size is larger than memory size");

        }

        public void fill_memory()
        {

            // sort holes with respect to start address
            holes.Sort((x, y) => x.start.CompareTo(y.start));


            for (int i = 0; i < holes.Count; i++)

            {
                int start = holes.ElementAt(i).start;
                int end = holes.ElementAt(i).end;



                // for the input hole inside a hole or same hole twice corner cases :'D
                bool add = true;
                for (int j = 0; j < filtered_holes.Count; j++)
                {
                    if ((start >= holes.ElementAt(j).start && end < holes.ElementAt(j).end)
                        || start == holes.ElementAt(j).start && end == holes.ElementAt(j).end)
                    {
                        add = false;
                        break;
                    }
                }




                // for the input hole inside a hole and the sum of the two is larger 
                // than the old one :'D


                for (int j = 0; j < filtered_holes.Count; j++)
                {
                    if (start >= holes.ElementAt(j).start && start < holes.ElementAt(j).end
                        && end > holes.ElementAt(j).end)

                    {
                        add = false;
                        filtered_holes.ElementAt(j).end = end;
                        break;
                    }
                }


                if (add)
                    filtered_holes.Add(holes.ElementAt(i));


            }

            // if end of a hole = start of a new hole update them to one hole
            for (int i = 0; i < filtered_holes.Count - 1; i++)
            {
                // lw la2et 2 holes wra b3d
                if (filtered_holes.ElementAt(i).end == filtered_holes.ElementAt(i + 1).start)
                {
                    // update el end bta3 el 2ola w 5ale ysawy end bta3 l tnya
                    filtered_holes.ElementAt(i).end = filtered_holes.ElementAt(i + 1).end;
                    filtered_holes.ElementAt(i).size = filtered_holes.ElementAt(i).end - filtered_holes.ElementAt(i).start;
                    filtered_holes.RemoveAt(i + 1);
                }
            }



            // start with hole and ends with hole case 
            if (filtered_holes.ElementAt(0).start == 0 &&
               filtered_holes.ElementAt(filtered_holes.Count - 1).end == memorySize)

                for (int i = 0; i < filtered_holes.Count - 1; i++)
                {
                    int start = filtered_holes.ElementAt(i).end;
                    int end = filtered_holes.ElementAt(i + 1).start;
                    string name = "process_ " + processes.Count.ToString();
                    MEM_BLOCK process = new MEM_BLOCK(start, end, name);
                    process.isHole = false;
                    processes.Add(process);
                }

            // start with hole and ends with process case
            if (filtered_holes.ElementAt(0).start == 0 &&
               filtered_holes.ElementAt(filtered_holes.Count - 1).end != memorySize)
            {
                for (int i = 0; i < filtered_holes.Count - 1; i++)
                {
                    int start = filtered_holes.ElementAt(i).end;
                    int end = filtered_holes.ElementAt(i + 1).start;
                    string name = "process_ " + processes.Count.ToString();
                    MEM_BLOCK process = new MEM_BLOCK(start, end, name);
                    process.isHole = false;
                    processes.Add(process);
                }

                //last process
                string lastName = "process_ " + processes.Count.ToString();
                MEM_BLOCK lastProcess = new MEM_BLOCK(filtered_holes.ElementAt(filtered_holes.Count - 1).end, memorySize, lastName);
                lastProcess.isHole = false;
                processes.Add(lastProcess);

            }

            // start with process and ends with hole case
            if (filtered_holes.ElementAt(0).start != 0 &&
               filtered_holes.ElementAt(filtered_holes.Count - 1).end == memorySize)



                for (int i = 0; i < filtered_holes.Count; i++)
                {
                    int start;
                    int end;

                    if (i == 0)
                    {
                        start = 0;
                        end = filtered_holes.ElementAt(0).start;

                    }

                    else
                    {
                        start = filtered_holes.ElementAt(i - 1).end;
                        end = filtered_holes.ElementAt(i).start;
                    }

                    string name = "process_ " + processes.Count.ToString();
                    MEM_BLOCK process = new MEM_BLOCK(start, end, name);
                    process.isHole = false;
                    processes.Add(process);
                }



            // start with process and ends with process case
            if (filtered_holes.ElementAt(0).start != 0 &&
               filtered_holes.ElementAt(filtered_holes.Count - 1).end != memorySize)

            {

                for (int i = 0; i < filtered_holes.Count; i++)
                {
                    int start;
                    int end;

                    if (i == 0)
                    {
                        start = 0;
                        end = filtered_holes.ElementAt(0).start;

                    }

                    else
                    {
                        start = filtered_holes.ElementAt(i - 1).end;
                        end = filtered_holes.ElementAt(i).start;
                    }

                    string name = "process_ " + processes.Count.ToString();
                    MEM_BLOCK process = new MEM_BLOCK(start, end, name);
                    process.isHole = false;
                    processes.Add(process);
                }

                //last process
                string lastName = "process_ " + processes.Count.ToString();
                MEM_BLOCK lastProcess = new MEM_BLOCK(filtered_holes.ElementAt(filtered_holes.Count - 1).end, memorySize, lastName);
                lastProcess.isHole = false;
                processes.Add(lastProcess);
            }





            // putting the holes and processes in one active memory list
            active_memory.AddRange(processes);
            active_memory.AddRange(filtered_holes);

            // sort the active_memory with respect to start address
            active_memory.Sort((x, y) => x.start.CompareTo(y.start));
        }


        public void add_process(string name, int size, string method)
        {
            if (method == "First Fit")
                MEMORY.sortBy = "First Fit";

            else if (method == "Best Fit")
                MEMORY.sortBy = "Best Fit";

            else if (method == "Worst Fit")
                MEMORY.sortBy = "Worst Fit";

            else
                MessageBox.Show("Please select method first");

            active_memory.Sort();

            for (int i = 0; i < active_memory.Count; i++)
            {
                if (active_memory.ElementAt(i).size > size && active_memory.ElementAt(i).isHole)
                {
                    // add the new process
                    int start = active_memory.ElementAt(i).start;
                    int end = start + size;
                    MEM_BLOCK process = new MEM_BLOCK(start, end, name);
                    process.isHole = false;



                    // update the new hole generated     
                    active_memory.ElementAt(i).start = end;
                    active_memory.ElementAt(i).size = active_memory.ElementAt(i).end - active_memory.ElementAt(i).start;

                    // add the process
                    active_memory.Add(process);

                    // sort the active_memory with respect to start address
                    active_memory.Sort((x, y) => x.start.CompareTo(y.start));


                    return;
                }


                else if (active_memory.ElementAt(i).size == size && active_memory.ElementAt(i).isHole)
                {
                    // add the new process
                    int start = active_memory.ElementAt(i).start;
                    int end = start + size;
                    MEM_BLOCK process = new MEM_BLOCK(start, end, name);
                    process.isHole = false;
                    active_memory.Add(process);

                    // delete the new hole generated 
                    active_memory.RemoveAt(i);



                    // sort the active_memory with respect to start address
                    active_memory.Sort((x, y) => x.start.CompareTo(y.start));


                    return;
                }




            }

            MessageBox.Show("No avialable space for this process, please deallocate a process and try again");

        }






        public void deallocate(string name, int number)
        {
            for (int i = 0; i < active_memory.Count; i++)
            {
                if (active_memory.ElementAt(i).name == name)
                {
                    active_memory.ElementAt(i).isHole = true;
                    active_memory.ElementAt(i).name = "hole_ " + number;
                    break;
                }
            }

            concatenate(); // 2 holes wra b3d
        }


        private void concatenate()
        {
            for (int i = 0; i < active_memory.Count - 1; i++)
            {

                if (!active_memory.ElementAt(i).isHole)
                    continue;

                int j = i + 1;
                while (j <= active_memory.Count - 1)
                {
                    if (active_memory.ElementAt(j).isHole)
                        j++;

                    else break;
                }

                if (j > active_memory.Count - 1) // l7d a5er el memory holes
                {
                    active_memory.ElementAt(i).end = memorySize;
                    active_memory.ElementAt(i).size = active_memory.ElementAt(i).end - active_memory.ElementAt(i).start;


                }

                else if (j > i + 1) // t7tya process
                {

                    active_memory.ElementAt(i).end = active_memory.ElementAt(j).start;
                    active_memory.ElementAt(i).size = active_memory.ElementAt(i).end - active_memory.ElementAt(i).start;


                }

                int n = j - 1 - i; // number of elements to be deleted 
                int r = j - 1; // ebd2 ems7 mn r


                for (int k = 0; k < n; k++)
                {
                    active_memory.RemoveAt(r);
                    r--;
                }






            }
        }


        public void compact(int name_number)
        {

            compact_memory.Clear();

            for (int i = 0; i < active_memory.Count; i++)
            {
                if (!active_memory.ElementAt(i).isHole)
                {
                    compact_memory.Add(active_memory.ElementAt(i));
                }
            }


            if (compact_memory.Count != 0)
            {


                compact_memory.ElementAt(0).start = 0;
                compact_memory.ElementAt(0).end = compact_memory.ElementAt(0).size;

                for (int i = 1; i <= compact_memory.Count - 1; i++)
                {
                    compact_memory.ElementAt(i).start = compact_memory.ElementAt(i - 1).end;
                    compact_memory.ElementAt(i).end = compact_memory.ElementAt(i).start + compact_memory.ElementAt(i).size;
                }





                string name = "hole_ " + name_number;
                MEM_BLOCK hole = new MEM_BLOCK(compact_memory.ElementAt(compact_memory.Count - 1).end, memorySize, name);
                hole.size = hole.end - hole.start;

                compact_memory.Add(hole);

                active_memory.Clear();
                active_memory.AddRange(compact_memory);

            }
        }



    }

}


