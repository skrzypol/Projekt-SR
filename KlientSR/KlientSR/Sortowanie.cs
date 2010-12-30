using System;
using System.Collections.Generic;
using System.Collections;
using System.Linq;
using System.Text;

namespace KlientSR
{
    class Sortowanie
    {
        int[] tab;
        int x;
        public ArrayList sortBubble(ArrayList lista)
        {
            int dlListy = lista.Count;
            int dl = ((string)lista[0]).Length;
            tab = new int[dlListy];
            x = dlListy;
            for (int k = 0; k < dlListy; k++)
            {
                tab[k] = int.Parse((string)lista[k]);
            }





            int i;
            int j;
            int temp;
            for (i = (x - 1); i >= 0; i--)
            {
                for (j = 1; j <= i; j++)
                {
                    if (tab[j - 1] > tab[j])
                    {
                        temp = tab[j - 1];
                        tab[j - 1] = tab[j];
                        tab[j] = temp;
                    }
                }
            }



            lista.Clear();
            string dane;
            int dl2;
            for (int k = 0; k < dlListy; k++)
            {
                dane = tab[k].ToString();
                dl2 = dane.Length;
                for (int l = dl2; l < dl; l++)
                    dane = "0" + dane;
                lista.Add(dane);
            }
            return lista;
        }
        public ArrayList sortHeap(ArrayList lista)
        {
            int dlListy = lista.Count;
            int dl = ((string)lista[0]).Length;
            tab = new int[dlListy];
            x = dlListy;
            for (int k = 0; k < dlListy; k++)
            {
                tab[k] = int.Parse((string)lista[k]);
            }



            int i;
            int temp;

            for (i = (x / 2) - 1; i >= 0; i--)
            {
                siftDown(i, x);
            }

            for (i = x - 1; i >= 1; i--)
            {
                temp = tab[0];
                tab[0] = tab[i];
                tab[i] = temp;
                siftDown(0, i - 1);
            }




            lista.Clear();
            string dane;
            int dl2;
            for (int k = 0; k < dlListy; k++)
            {
                dane = tab[k].ToString();
                dl2 = dane.Length;
                for (int l = dl2; l < dl; l++)
                    dane = "0" + dane;
                lista.Add(dane);
            }
            return lista;
        }
        public ArrayList sortInsertion(ArrayList lista)
        {
            int dlListy = lista.Count;
            int dl = ((string)lista[0]).Length;
            tab = new int[dlListy];
            x = dlListy;
            for (int k = 0; k < dlListy; k++)
            {
                tab[k] = int.Parse((string)lista[k]);
            }



            int i;
            int j;
            int index;

            for (i = 1; i < x; i++)
            {
                index = tab[i];
                j = i;

                while ((j > 0) && (tab[j - 1] > index))
                {
                    tab[j] = tab[j - 1];
                    j = j - 1;
                }

                tab[j] = index;
            }

            lista.Clear();
            string dane;
            int dl2;
            for (int k = 0; k < dlListy; k++)
            {
                dane = tab[k].ToString();
                dl2 = dane.Length;
                for (int l = dl2; l < dl; l++)
                    dane = "0" + dane;
                lista.Add(dane);
            }
            return lista;
        }
        public ArrayList sortMerge(ArrayList lista)
        {
            int dlListy = lista.Count;
            int dl = ((string)lista[0]).Length;
            tab = new int[dlListy];
            x = dlListy;
            for (int k = 0; k < dlListy; k++)
            {
                tab[k] = int.Parse((string)lista[k]);
            }


            m_sort(0, x - 1);


            lista.Clear();
            string dane;
            int dl2;
            for (int k = 0; k < dlListy; k++)
            {
                dane = tab[k].ToString();
                dl2 = dane.Length;
                for (int l = dl2; l < dl; l++)
                    dane = "0" + dane;
                lista.Add(dane);
            }
            return lista;
        }
        public ArrayList sortQuick(ArrayList lista)
        {
            int dlListy = lista.Count;
            int dl = ((string)lista[0]).Length;
            tab = new int[dlListy];
            x = dlListy;
            for (int k = 0; k < dlListy; k++)
            {
                tab[k] = int.Parse((string)lista[k]);
            }

            q_sort(0, x - 1);

            lista.Clear();
            string dane;
            int dl2;
            for (int k = 0; k < dlListy; k++)
            {
                dane = tab[k].ToString();
                dl2 = dane.Length;
                for (int l = dl2; l < dl; l++)
                    dane = "0" + dane;
                lista.Add(dane);
            }
            return lista;
        }
        public ArrayList sortSelection(ArrayList lista)
        {
            int dlListy = lista.Count;
            int dl = ((string)lista[0]).Length;
            tab = new int[dlListy];
            x = dlListy;
            for (int k = 0; k < dlListy; k++)
            {
                tab[k] = int.Parse((string)lista[k]);
            }

            int i, j;
            int min, temp;

            for (i = 0; i < x - 1; i++)
            {
                min = i;

                for (j = i + 1; j < x; j++)
                {
                    if (tab[j] < tab[min])
                    {
                        min = j;
                    }
                }

                temp = tab[i];
                tab[i] = tab[min];
                tab[min] = temp;
            }

            lista.Clear();
            string dane;
            int dl2;
            for (int k = 0; k < dlListy; k++)
            {
                dane = tab[k].ToString();
                dl2 = dane.Length;
                for (int l = dl2; l < dl; l++)
                    dane = "0" + dane;
                lista.Add(dane);
            }
            return lista;
        }
        public ArrayList sortShell(ArrayList lista)
        {
            int dlListy = lista.Count;
            int dl = ((string)lista[0]).Length;
            tab = new int[dlListy];
            x = dlListy;
            for (int k = 0; k < dlListy; k++)
            {
                tab[k] = int.Parse((string)lista[k]);
            }


            int i, j, increment, temp;

            increment = 3;

            while (increment > 0)
            {
                for (i = 0; i < x; i++)
                {
                    j = i;
                    temp = tab[i];

                    while ((j >= increment) && (tab[j - increment] > temp))
                    {
                        tab[j] = tab[j - increment];
                        j = j - increment;
                    }

                    tab[j] = temp;
                }

                if (increment / 2 != 0)
                {
                    increment = increment / 2;
                }
                else if (increment == 1)
                {
                    increment = 0;
                }
                else
                {
                    increment = 1;
                }
            }

            lista.Clear();
            string dane;
            int dl2;
            for (int k = 0; k < dlListy; k++)
            {
                dane = tab[k].ToString();
                dl2 = dane.Length;
                for (int l = dl2; l < dl; l++)
                    dane = "0" + dane;
                lista.Add(dane);
            }
            return lista;
        }





        void q_sort(int left, int right)
        {
            int pivot, l_hold, r_hold;

            l_hold = left;
            r_hold = right;
            pivot = tab[left];

            while (left < right)
            {
                while ((tab[right] >= pivot) && (left < right))
                {
                    right--;
                }

                if (left != right)
                {
                    tab[left] = tab[right];
                    left++;
                }

                while ((tab[left] <= pivot) && (left < right))
                {
                    left++;
                }

                if (left != right)
                {
                    tab[right] = tab[left];
                    right--;
                }
            }

            tab[left] = pivot;
            pivot = left;
            left = l_hold;
            right = r_hold;

            if (left < pivot)
            {
                q_sort(left, pivot - 1);
            }

            if (right > pivot)
            {
                q_sort(pivot + 1, right);
            }
        }
        void m_sort(int left, int right)
        {
            int mid;

            if (right > left)
            {
                mid = (right + left) / 2;
                m_sort(left, mid);
                m_sort(mid + 1, right);

                merge(left, mid + 1, right);
            }
        }
        void merge(int left, int mid, int right)
        {
            int[] b = new int[x];
            int i, left_end, num_elements, tmp_pos;

            left_end = mid - 1;
            tmp_pos = left;
            num_elements = right - left + 1;

            while ((left <= left_end) && (mid <= right))
            {
                if (tab[left] <= tab[mid])
                {
                    b[tmp_pos] = tab[left];
                    tmp_pos = tmp_pos + 1;
                    left = left + 1;
                }
                else
                {
                    b[tmp_pos] = tab[mid];
                    tmp_pos = tmp_pos + 1;
                    mid = mid + 1;
                }
            }

            while (left <= left_end)
            {
                b[tmp_pos] = tab[left];
                left = left + 1;
                tmp_pos = tmp_pos + 1;
            }

            while (mid <= right)
            {
                b[tmp_pos] = tab[mid];
                mid = mid + 1;
                tmp_pos = tmp_pos + 1;
            }

            for (i = 0; i < num_elements; i++)
            {
                tab[right] = b[right];
                right = right - 1;
            }
        }
        void siftDown(int root, int bottom)
        {
            bool done = false;
            int maxChild;
            int temp;

            while ((root * 2 <= bottom) && (!done))
            {
                if (root * 2 == bottom)
                    maxChild = root * 2;
                else if (tab[root * 2] > tab[root * 2 + 1])
                    maxChild = root * 2;
                else
                    maxChild = root * 2 + 1;

                if (tab[root] < tab[maxChild])
                {
                    temp = tab[root];
                    tab[root] = tab[maxChild];
                    tab[maxChild] = temp;
                    root = maxChild;
                }
                else
                {
                    done = true;
                }
            }
        }
    }
}
