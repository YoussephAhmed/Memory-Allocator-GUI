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
        
        public Form1()
        {
            InitializeComponent();
            method.Items.Add("First Fit");
            method.Items.Add("Best Fit");
            method.Items.Add("Worst Fit");

        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

   

        private void show_output_Click(object sender, EventArgs e)
        {
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


        }

        private void output_text_TextChanged(object sender, EventArgs e)
        {
            user_method = method.Text;
        }

        private void mem_size_Click(object sender, EventArgs e)
        {
           memory.memorySize = int.Parse(memSize.Text);
           memSize.Clear();
        }

      
        private void add_hole_Click(object sender, EventArgs e)
        {

            int hallEnd = int.Parse(holeStart.Text) + int.Parse(holeSize.Text) - 1;
            string name = "hole_ " + memory.holes.Count.ToString();
            memory.add_hole(int.Parse(holeStart.Text), hallEnd, name);
            holeStart.Clear();
            holeSize.Clear();
        }

        private void run_Click(object sender, EventArgs e)
        {
            memory.fill_memory();
          
        }

        private void label4_Click(object sender, EventArgs e)
        {

        }

        private void add_process_Click(object sender, EventArgs e)
        {
            string name = process_name.Text;
            int size = int.Parse(process_size.Text);
            user_method = method.Text;
            memory.add_process(name, size, user_method);
            process_name.Clear();
            process_size.Clear();
        }

        private void method_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void deallocate_Click(object sender, EventArgs e)
        {
            string name = active_processes.Text;
            memory.deallocate(name);
            
            active_processes.Items.Clear();
            for (int i = 0; i < memory.active_memory.Count; i++)
            {
                if (memory.active_memory.ElementAt(i).isHole == false)
                    active_processes.Items.Add(memory.active_memory.ElementAt(i).name);

            }

              
        }
    }


    class MEM_BLOCK : IComparable<MEM_BLOCK>
    {

        public int start;
        public int end;
        public string name;
        public bool isHole;
        public int size;



        public MEM_BLOCK(int start , int end , string name)//constructor
        {
            this.start = start;
            this.end = end;
            this.name = name;
            isHole = true;
            size = end - start +1 ;

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


            // if end of a hole = start of a new hole update them to one hole
            for (int i = 0; i < holes.Count - 1; i++)
            {
                // lw la2et 2 holes wra b3d
                if (holes.ElementAt(i).end == holes.ElementAt(i + 1).start)
                {
                    // update el end bta3 el 2ola w 5ale ysawy end bta3 l tnya
                    holes.ElementAt(i).end = holes.ElementAt(i + 1).end;
                    holes.ElementAt(i).size = holes.ElementAt(i).end - holes.ElementAt(i).start -1;
                    holes.RemoveAt(i + 1);
                }
            }



            // start with hole and ends with hole case 
            if (holes.ElementAt(0).start == 0 && 
               holes.ElementAt(holes.Count - 1).end == memorySize -1 )
              
                    for (int i = 0; i < holes.Count-1; i++)
                    {
                    int start = holes.ElementAt(i).end;
                    int end = holes.ElementAt(i + 1).start;
                    string name = "process_ " + processes.Count.ToString();
                    MEM_BLOCK process = new MEM_BLOCK(start, end, name);
                    process.isHole = false;
                    processes.Add(process);
                    }

            // start with hole and ends with process case
            if (holes.ElementAt(0).start == 0 &&
               holes.ElementAt(holes.Count - 1).end != memorySize - 1)
            {
                for (int i = 0; i < holes.Count - 1; i++)
                {
                    int start = holes.ElementAt(i).end;
                    int end = holes.ElementAt(i + 1).start;
                    string name = "process_ " + processes.Count.ToString();
                    MEM_BLOCK process = new MEM_BLOCK(start, end, name);
                    process.isHole = false;
                    processes.Add(process);
                }

                //last process
                string lastName = "process_ " + processes.Count.ToString();
                MEM_BLOCK lastProcess = new MEM_BLOCK(holes.ElementAt(holes.Count - 1).end, memorySize - 1, lastName);
                lastProcess.isHole = false;
                processes.Add(lastProcess);

            }

            // start with process and ends with hole case
            if (holes.ElementAt(0).start != 0 &&
               holes.ElementAt(holes.Count - 1).end == memorySize - 1)



                for (int i = 0; i < holes.Count; i++)
                {
                    int start;
                    int end;

                    if (i == 0)
                    {
                        start = 0;
                        end = holes.ElementAt(0).start;

                    }

                    else
                    {
                        start = holes.ElementAt(i-1).end;
                        end = holes.ElementAt(i).start;
                    } 
                    
                    string name = "process_ " + processes.Count.ToString();
                    MEM_BLOCK process = new MEM_BLOCK(start, end, name);
                    process.isHole = false;
                    processes.Add(process);
                }



            // start with process and ends with process case
            if (holes.ElementAt(0).start != 0 &&
               holes.ElementAt(holes.Count - 1).end != memorySize - 1)

            {

                for (int i = 0; i < holes.Count; i++)
                {
                    int start;
                    int end;

                    if (i == 0)
                    {
                        start = 0;
                        end = holes.ElementAt(0).start;

                    }

                    else
                    {
                        start = holes.ElementAt(i - 1).end;
                        end = holes.ElementAt(i).start;
                    }

                    string name = "process_ " + processes.Count.ToString();
                    MEM_BLOCK process = new MEM_BLOCK(start, end, name);
                    process.isHole = false;
                    processes.Add(process);
                }

                //last process
                string lastName = "process_ " + processes.Count.ToString();
                MEM_BLOCK lastProcess = new MEM_BLOCK(holes.ElementAt(holes.Count - 1).end, memorySize - 1, lastName);
                lastProcess.isHole = false;
                processes.Add(lastProcess);
            }





            // putting the holes and processes in one active memory list
            active_memory.AddRange(processes);
            active_memory.AddRange(holes);
           
            // sort the active_memory with respect to start address
            active_memory.Sort((x, y) => x.start.CompareTo(y.start));
        }


        public void add_process(string name , int size , string method)
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
                    if(active_memory.ElementAt(i).size > size && active_memory.ElementAt(i).isHole) 
                    {
                        // add the new process
                        int start = active_memory.ElementAt(i).start;
                        int end = start + size-1;
                        MEM_BLOCK process = new MEM_BLOCK(start, end, name);
                        process.isHole = false;
                        active_memory.Add(process);

                        // add the new hole generated 
                        
                        active_memory.ElementAt(i).start = end;
                        active_memory.ElementAt(i).size = end - active_memory.ElementAt(i).start - 1;
                

                    // sort the active_memory with respect to start address
                    active_memory.Sort((x, y) => x.start.CompareTo(y.start));
                   
                    
                    return;
                    }


                    else if (active_memory.ElementAt(i).size == size && active_memory.ElementAt(i).isHole)
                    {
                        // add the new process
                        int start = active_memory.ElementAt(i).start;
                        int end = start + size - 1;
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






        public void deallocate(string name)
        {
            for(int i = 0; i < active_memory.Count; i++)
            {
                if (active_memory.ElementAt(i).name == name)
                {
                    active_memory.ElementAt(i).isHole = true;
                    active_memory.ElementAt(i).name = "hole_ ";
                    break;
                }
            }

            concatenate(); // 2 holes wra b3d
        }


        private void concatenate()
        {
            for(int i = 0; i < active_memory.Count-1; i++)
            {
                // lw la2et 2 holes wra b3d
                if(active_memory.ElementAt(i).isHole && active_memory.ElementAt(i+1).isHole)
                {
                    // update el end bta3 el 2ola w 5ale ysawy end bta3 l tnya
                    active_memory.ElementAt(i).end = active_memory.ElementAt(i + 1).end;
                    active_memory.ElementAt(i).size = active_memory.ElementAt(i).end - active_memory.ElementAt(i).start - 1;
                    active_memory.RemoveAt(i + 1);
                }
            }
        }

        }

    }


