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



    }

}
