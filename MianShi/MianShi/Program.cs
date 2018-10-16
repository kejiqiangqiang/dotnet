using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MianShi
{


    class Program
    {
        #region 静态类加载过程
        //static void Main(string[] args)
        //{
        //    Console.WriteLine("X ={0},Y ={1}", A.X, B.Y);
        //}
        //class A

        //{

        //    public static int X;

        //    static A()
        //    {

        //        X = B.Y + 1;

        //    }

        //}

        //class B

        //{

        //    public static int Y = A.X + 1;

        //    static B() { }
        //}
        #endregion

        #region 继承重写虚方法--子类继承父类方法 F2 （单纯的继承，不是重写），而此父类方法又调用父类的虚方法F1，同时子类又重写父类的虚方法 F1 ，若通过子类调用继承的方法F2，那么F2中执行到调用 F1 时，调用的是子类重写的 F1 方法，而不是父类的虚方法 F1
        //public class A
        //{
        //    public virtual void Fun1(int i)
        //    {
        //        Console.WriteLine(i);
        //    }

        //    public void Fun2(A a)
        //    {
        //        a.Fun1(1);
        //        Fun1(5);
        //    }
        //}

        //public class B : A
        //{
        //    public override void Fun1(int i)
        //    {
        //        base.Fun1(i + 1);
        //    }
        //}
        //public static void Main()
        //{
        //    B b = new B();
        //    A a = new A();
        //    a.Fun2(b);//2,5
        //    b.Fun2(a);//1,6
        //}
        #endregion

        #region 子类调用父类构造方法+重写虚方法
        public class Person
        {
            protected int Age = 10;
            protected Person()
            {
                this.Show();
            }
            protected virtual void Show()
            {
                Console.WriteLine(this.Age);
            }
        }
        public class Employee : Person
        {
            public Employee()
            {
                base.Age = 20;
            }
            protected override void Show()
            {
                base.Show();
            }
        }
        //static void Main(string[] args)
        //{
        //    Employee e = new Employee(); Console.ReadKey();//10
        //}
        #endregion

        #region 堆、堆排序、TopK

        //static void Main(string[] args)
        //{
        //    int[] arr = new int[] { 6, 2, 3, 1, 5, 4, 9, 8, 7 };

        //    //heap_create(arr, arr.Length);

        //    //heap_create1(arr, arr.Length);

        //    //heap_sort(arr,arr.Length);

        //    int k = 3;
        //    int[] res = new int[3];
        //    topk(arr,arr.Length,res,k);

        //    Console.WriteLine(string.Join(",", arr));
        //}

        // Coding Study - Heap
        // Heap is a sepcial complete binary tree(CBT)

        /*    Heap sketch is a CBT, but stored in Array
         *        9 ---> maxtop         7                 7
         *       / \                   / \               / \
         *      /   \                 /   \             /   \
         *     7     8               4     8           4     5
         *    / \   / \             / \               /     /
         *   /   \ /   \           /   \             /     /
         *  5    3 2    4         3     5           3     6
         *
         *      (1)                  (2)                (3)
         *  maxtop heap       not maxtop(mintop)    not heap(CBT)
         * */

        // Method 1
        // Create (Initialize) Heap, from top to bottom
        static void heap_create(int[] arr, int n)
        {
            int i;      // from top to bottom
            for (i = 1; i < n; heap_adjust(arr, i++)) ;
        }



        static void heap_adjust(int[] arr, int c)
        {   // c - children, p - parent
            int p = (c - 1) >> 1, temp;
            // heap adjust from maxtop, from bottom to top
            for (; p > -1 && arr[p] < arr[c]; c = p, p = (c - 1) >> 1)
            {
                // swap arr[p] and arr[c]
                temp = arr[p];
                arr[p] = arr[c];
                arr[c] = temp;
            }
        }   // Time O(logn)



        // Method 2
        // Create (Initialize) Heap, from bottom to top
        static void heap_create1(int[] arr, int n)
        {
            int i;      // from bottom to top
            for (i = (n >> 1) - 1; i > -1; heap_adjust(arr, i--, n)) ;
        }



        static void heap_adjust(int[] arr, int p, int n)
        {   // c - children, p - parent
            int maxid = p, temp;
            // heap_adjust for maxtop, from top to bottom
            for (; p < (n >> 1); p = maxid)
            {
                if ((p << 1) + 1 < n && arr[(p << 1) + 1] > arr[maxid])
                    maxid = (p << 1) + 1;
                if ((p << 1) + 2 < n && arr[(p << 1) + 2] > arr[maxid])
                    maxid = (p << 1) + 2;
                if (maxid == p) break;
                // swap arr[maxid] and arr[p]
                temp = arr[maxid];
                arr[maxid] = arr[p];
                arr[p] = temp;
            }
        }   // Time O(logn)



        // Heap Sort - ascending order
        static void heap_sort(int[] arr, int n)
        {
            int i, temp;
            // init maxtop heap, using method 2 (from bottom to top)
            for (i = (n >> 1) - 1; i > -1; heap_adjust(arr, i--, n)) ;
            for (i = n - 1; i > 0; heap_adjust(arr, 0, i--))
            {
                // mv heap top to end (heap top is max)
                // swap arr[0] and arr[i]
                temp = arr[0];
                arr[0] = arr[i];
                arr[i] = temp;
            }
        }   // Time O(nlogn)



        // TopK problem : find max k (or min k) elements from unordered set
        // eg. find min k elements from arr[], stored in res[]
        static void topk(int[] arr, int n, int[] res, int k)
        {
            int i;      // copy and k elements to res
            for (i = 0; i < k; res[i] = arr[i], ++i) ;
            // make maxtop heap for res[]
            for (i = (k >> 1) - 1; i > -1; heap_adjust(res, i--, k)) ;
            for (i = k; i < n; ++i)
            {
                if (res[0] <= arr[i]) continue;
                // now arr[i] < heap top
                res[0] = arr[i];
                heap_adjust(res, 0, k);
            }
        }   // Time O(nlogk)



        #endregion

        #region 数组倒置、翻转

        /// <summary>
        /// 倒置
        /// </summary>
        /// <param name="arr"></param>
        /// <param name="n"></param>
        static void reverse(int[] arr, int n)
        {
            for (int i = 0; i < n / 2; i++)
            {
                //一般交换操作
                //int temp = arr[i];
                //arr[i] = arr[n-1-i];
                //arr[n - 1 - i] = temp;

                //异或交换操作
                //A=A^B;B=A^B;A=A^B;
                arr[i] ^= arr[n - 1 - i];
                arr[n - 1 - i] = arr[i] ^ arr[n - 1 - i];
                arr[i] ^= arr[n - 1 - i];
            }
        }

        /// <summary>
        /// 倒置
        /// </summary>
        /// <param name="arr"></param>
        /// <param name="n"></param>
        static void reverse(int[] arr, int left, int right)
        {
            if (left < 0 || right < 0)
            {
                return;
            }
            while (left < right)
            {
                //一般交换操作
                //int temp = arr[i];
                //arr[i] = arr[n-1-i];
                //arr[n - 1 - i] = temp;

                //异或交换操作
                //A=A^B;B=A^B;A=A^B;
                arr[left] ^= arr[right];
                arr[right] = arr[left] ^ arr[right];
                arr[left] ^= arr[right];

                left++; right--;
            }
        }

        /// <summary>
        /// 翻转
        /// </summary>
        /// <param name="arr"></param>
        /// <param name="n"></param>
        /// <param name="r"></param>
        static void rotate(int[] arr, int n, int r)
        {
            r %= n;
            //前半部分倒置
            reverse(arr, r);
            //后半部分倒置
            //reverse(arr+r, n-r);//arr+r指针
            reverse(arr, r, n - 1);
            //整体倒置
            reverse(arr, n);
        }


        #region 字符串逆置

        public static string reverseRecursive(string s)
        {
            int length = s.Length;
            if (length <= 1)
                return s;
            string left = s.Substring(0, length / 2);
            string right = s.Substring(length / 2, length - length / 2);
            string afterReverse = reverseRecursive(right) + reverseRecursive(left);
            return afterReverse;
        }
        #endregion

        //static void Main(string[] args)
        //{
        //    int[] arr = { 1,2,3,4,5,6,7};
        //    //reverse(arr,arr.Length);
        //    rotate(arr,arr.Length,5);
        //    var s = reverseRecursive("12345abc");
        //}

        #endregion

        #region 排序

        static void Main(string[] args)
        {
            int[] arr = new int[] { 6, 2, 3, 1, 5, 4, 9, 8, 7 };

            //BubbleSort(arr);

            //QuickSort(arr, 0, arr.Length - 1);

            //SelectSort(arr, arr.Length);

            //int[] temp = new int[arr.Length];
            //MergeSort(arr, 0, arr.Length - 1, temp);



        }
        /*
         * 1.冒泡排序--O(n^2)
         * */
        static void BubbleSort(int[] a)
        {
            int temp;
            for (int i = 0; i < a.Length; i++)
            {
                for (int j = i+1; j < a.Length; j++)
                {
                    if (a[i]>a[j])
                    {
                        temp = a[i];
                        a[i] = a[j];
                        a[j] = temp;
                    }
                }
            }
        }
        /*
         * 冒泡排序改进--鸡尾酒排序--O(n^2)
         * */
        static void CocktailSort(int[] a, int n)
        {
            int left = 0;                            // 初始化边界
            int right = n - 1;
            while (left < right)
            {
                for (int i = left; i < right; i++)   // 前半轮,将最大元素放到后面
                {
                    if (a[i] > a[i + 1])
                    {
                        Swap(a, i, i + 1);
                    }
                }
                right--;
                for (int i = right; i > left; i--)   // 后半轮,将最小元素放到前面
                {
                    if (a[i - 1] > a[i])
                    {
                        Swap(a, i - 1, i);
                    }
                }
                left++;
            }
        }
        static void Swap(int[] a,int i,int j)
        {
            int temp = a[i];
            a[i] = a[j];
            a[j] = temp;
        }

        /*
         * 2.快速排序--O(nlog2n)
         * */
        static void QuickSort(int[] a,int left,int right)
        {
            if (left >= right)
            {
                return;
            }
            int i = left;
            int j = right;
            int key = a[i];
            while (i < j)
            {
                while (i < j && a[j] >= key)
                {
                    j--;
                }
                a[i] = a[j];
                while (i < j && a[i] <= key)
                {
                    i++;
                }
                a[j] = a[i];
            }
            a[i] = key;

            QuickSort(a, left, i - 1);
            QuickSort(a, i + 1, right);
        }

        /*
         * 3.选择排序--O(n^2)
         * */

        static void SelectSort(int[] a,int n)
        {
            for (int i = 0; i < n-1; i++)
            {
                int min = i;
                for (int j = i + 1; j < n; j++)
                {
                    if (a[min]>a[j])
                    {
                        min = j;
                    }
                }
                if (i!=min)
                {
                    int temp = a[i];
                    a[i] = a[min];
                    a[min] = temp;
                }
            }
        }

        /*
         * 插入排序--
         * */


        /*
         * 归并排序--回溯
         * */
        static void MergeSort(int[] a, int first, int last, int[] temp)
        {
            if (first < last)
            {
                int mid = (first + last) / 2;
                MergeSort(a, first, mid, temp);//回溯
                MergeSort(a, mid + 1, last, temp);//回溯
                MergeArray(a, first, mid, last, temp);//回溯
            }

        }
        /*
         * 合并有序数组
         * */
        static void MergeArray(int[] a,int first,int mid,int last,int[] temp)
        {
            int i = first;
            int j = mid + 1;
            int k = 0;
            while (i<=mid&&j<=last)
            {
                if (a[i]<=a[j])
                {
                    temp[k++] = a[i++];
                }
                else
                {
                    temp[k++] = a[j++];
                }
            }
            while (i <= mid)
            {
                temp[k++] = a[i++];
            }
            while (j <= last)
            {
                temp[k++] = a[j++];
            }

            for (i = 0; i < k; i++)
            {
                a[first + i] = temp[i];
            }
        }

        /*
         * 堆排序-如上所示
         * */

        #endregion

        #region 小测试

        #endregion
    }

}
