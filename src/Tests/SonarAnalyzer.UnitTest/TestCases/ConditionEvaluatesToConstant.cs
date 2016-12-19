﻿using System;
using System.Diagnostics;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Tests.Diagnostics
{
    public class ConditionEvaluatesToConstant
    {
        public void Method1()
        {
            var b = true;
            if (b) // Noncompliant {{Change this condition so that it does not always evaluate to "true".}}
//              ^
            {
                Console.WriteLine();
            }
            else
            {
                Console.WriteLine();
            }

            Console.WriteLine();
        }

        public void Method2()
        {
            var b = true;
            if (b) // Noncompliant
            {
                Console.WriteLine();
            }

            if (!b) // Noncompliant
            {
                Console.WriteLine();
            }

            Console.WriteLine();
        }

        public void Method2Literals()
        {
            if (true) // Compliant
            {
                Console.WriteLine();
            }

            if (false) // Compliant
            {
                Console.WriteLine();
            }

            Console.WriteLine();
        }

        public void Method3()
        {
            bool b;
            TryGet(out b);
            if (b) { }
        }
        private void TryGet(out bool b) { b = false; }

        public void Method4()
        {
            var b = true;
            while (b) // Noncompliant
            {
                Console.WriteLine();
            }

            Console.WriteLine();
        }

        public void Method5(bool cond)
        {
            while (cond)
            {
                Console.WriteLine();
            }

            var b = true;
            while (b) // Noncompliant
            {
                Console.WriteLine();
            }

            Console.WriteLine();
        }

        public void Method6(bool cond)
        {
            var i = 10;
            while (i < 20)
            {
                i = i + 1;
            }

            var b = true;
            while (b) // Noncompliant
            {
                Console.WriteLine();
            }

            Console.WriteLine();
        }

        public void Method7(bool cond)
        {
            while (true) // Not reporting on this
            {
                Console.WriteLine();
            }

            Console.WriteLine();
        }

        public void Method8(bool cond)
        {
            foreach (var item in new int[][] { { 1, 2, 3 } })
            {
                foreach (var i in item)
                {
                    Console.WriteLine();
                }
            }
        }

        public void Method9_For(bool cond)
        {
            for (;;) // Not reporting on this
            {

            }
        }

        public void Method_Switch()
        {
            int i = 10;
            bool b = true;
            switch (i)
            {
                case 1:
                default:
                case 2:
                    b = false;
                    break;
                case 3:
                    b = false;
                    break;
            }

            if (b) // Noncompliant {{Change this condition so that it does not always evaluate to "false".}}
            {

            }
            else
            { }
        }

        public void Method_Switch_NoDefault()
        {
            int i = 10;
            bool b = true;
            switch (i)
            {
                case 1:
                case 2:
                    b = false;
                    break;
            }

            if (b)
            {

            }
            else
            {

            }
        }

        public void Method_Switch_Learn(bool cond)
        {
            switch (cond)
            {
                case true:
                    if (cond) // Non-compliant, we don't care it's very rare
                    {
                        Console.WriteLine();
                    }
                    break;
            }
        }

        public bool Property1
        {
            get
            {
                var a = new Action(() =>
                {
                    var b = true;
                    if (b) // Noncompliant
                    {
                        Console.WriteLine();
                    }
                    else
                    {
                        Console.WriteLine();
                    }
                });
                return true;
            }
            set
            {
                value = true;
                if (value) // Noncompliant
//                  ^^^^^
                {
                    Console.WriteLine();
                }
                else
                {
                    Console.WriteLine();
                }
            }
        }

        public void Method_Complex()
        {
            bool guard1 = true;
            bool guard2 = true;
            bool guard3 = true;

            while (GetCondition())
            {
                if (guard1)
                {
                    guard1 = false;
                }
                else
                {
                    if (guard2) // Noncompliant, false-positive
                    {
                        guard2 = false;
                    }
                    else
                    {
                        guard3 = false;
                    }
                }
            }

            if (guard3) // Noncompliant, false-positive, kept only to show that problems with loops can cause issues outside the loop
            {
                Console.WriteLine();
            }
        }

        public void Method_Complex_2()
        {
            var x = false;
            var y = false;

            while (GetCondition())
            {
                while (GetCondition())
                {
                    if (x)
                    {
                        if (y) // Noncompliant, false-positive
                        {
                        }
                    }
                    y = true;
                }
                x = true;
            }
        }

        public void M()
        {
            var o1 = GetObject();
            object o2 = null;
            if (o1 != null)
            {
                if (o1.ToString() != null)
                {
                    o2 = new object();
                }
            }
            if (o2 == null)
            {

            }
        }

        public void NullableStructs()
        {
            int? i = null;

            if (i == null) // Noncompliant, always true
            {
                Console.WriteLine(i);
            }

            i = new Nullable<int>();
            if (i == null) // Noncompliant
            { }

            int ii = 4;
            if (ii == null) // Noncompliant, always false
            {
                Console.WriteLine(ii);
            }
        }

        private static bool GetCondition()
        {
            return true;
        }

        public void Lambda()
        {
            var fail = false;
            Action a = new Action(() => { fail = true; });
            a();
            if (fail) // This is compliant, we don't know anything about 'fail'
            {
            }
        }

        public void Constraint(bool cond)
        {
            var a = cond;
            var b = a;
            if (a)
            {
                if (b) // Noncompliant {{Change this condition so that it does not always evaluate to "true".}}
                {

                }
            }
        }

        public void Stack(bool cond)
        {
            var a = cond;
            var b = a;
            if (!a)
            {
                if (b) // Noncompliant {{Change this condition so that it does not always evaluate to "false".}}
                {

                }
            }

            var fail = false;
            Action a = new Action(() => { fail = true; });
            a();
            if (!fail) // This is compliant, we don't know anything about 'fail'
            {
            }
        }

        public void BooleanBinary(bool a, bool b)
        {
            if (a & !b)
            {
                if (a) { } // Noncompliant {{Change this condition so that it does not always evaluate to "true".}}
                if (b) { } // Noncompliant {{Change this condition so that it does not always evaluate to "false".}}
            }

            if (!(a | b))
            {
                if (a) { } // Noncompliant {{Change this condition so that it does not always evaluate to "false".}}
            }

            if (a ^ b)
            {
                if (!a ^ !b) { } // Noncompliant {{Change this condition so that it does not always evaluate to "true".}}
            }

            a = false;
            if (a & b) { } // Noncompliant {{Change this condition so that it does not always evaluate to "false".}}

            a &= true;
            if (a) { } // Noncompliant {{Change this condition so that it does not always evaluate to "false".}}

            a |= true;
            if (a) { } // Noncompliant {{Change this condition so that it does not always evaluate to "true".}}

            a ^= true;
            if (a) { } // Noncompliant {{Change this condition so that it does not always evaluate to "false".}}

            a ^= true;
            if (a) { } // Noncompliant {{Change this condition so that it does not always evaluate to "true".}}
        }

        public void IsAsExpression()
        {
            object o = new object();
            if (o is object) { }
            var oo = o as object;
            if (oo == null) { }

            o = null;
            if (o is object) { } // Noncompliant {{Change this condition so that it does not always evaluate to "false".}}
            oo = o as object;
            if (oo == null) { } // Noncompliant {{Change this condition so that it does not always evaluate to "true".}}
        }

        public void Equals(bool b)
        {
            var a = true;
            if (a == b)
            {
                if (b) { }  // Noncompliant {{Change this condition so that it does not always evaluate to "true".}}
            }
            else
            {
                if (b) { }  // Noncompliant {{Change this condition so that it does not always evaluate to "false".}}
            }

            if (!(a == b))
            {
                if (b) { }  // Noncompliant {{Change this condition so that it does not always evaluate to "false".}}
            }
            else
            {
                if (b) { }  // Noncompliant {{Change this condition so that it does not always evaluate to "true".}}
            }
        }

        public void NotEquals(bool b)
        {
            var a = true;
            if (a != b)
            {
                if (b) { }  // Noncompliant {{Change this condition so that it does not always evaluate to "false".}}
            }
            else
            {
                if (b) { }  // Noncompliant {{Change this condition so that it does not always evaluate to "true".}}
            }

            if (!(a != b))
            {
                if (b) { }  // Noncompliant {{Change this condition so that it does not always evaluate to "true".}}
            }
            else
            {
                if (b) { }  // Noncompliant {{Change this condition so that it does not always evaluate to "false".}}
            }
        }

        public void EqRelations(bool a, bool b)
        {
            if (a == b)
            {
                if (b == a) { }    // Noncompliant {{Change this condition so that it does not always evaluate to "true".}}
                if (b == !a) { }   // Noncompliant {{Change this condition so that it does not always evaluate to "false".}}
                if (!b == !!a) { } // Noncompliant {{Change this condition so that it does not always evaluate to "false".}}
                if (!(a == b)) { } // Noncompliant {{Change this condition so that it does not always evaluate to "false".}}
            }
            else
            {
                if (b != a) { }    // Noncompliant {{Change this condition so that it does not always evaluate to "true".}}
                if (b != !a) { }   // Noncompliant {{Change this condition so that it does not always evaluate to "false".}}
                if (!b != !!a) { } // Noncompliant {{Change this condition so that it does not always evaluate to "false".}}
            }

            if (a != b)
            {
                if (b == a) { } // Noncompliant {{Change this condition so that it does not always evaluate to "false".}}
            }
            else
            {
                if (b != a) { } // Noncompliant {{Change this condition so that it does not always evaluate to "false".}}
            }
        }

        public void RelationshipWithConstraint(bool a, bool b)
        {
            if (a == b && a) { if (b) { } } // Noncompliant {{Change this condition so that it does not always evaluate to "true".}}
//                                 ^
            if (a != b && a) { if (b) { } } // Noncompliant {{Change this condition so that it does not always evaluate to "false".}}
//                                 ^
            if (a && b) { if (a == b) { } } // Noncompliant
//                            ^^^^^^
            if (a && b && a == b) { } // Noncompliant
//                        ^^^^^^

            a = true;
            b = false;
            if (a &&        // Noncompliant
                b)          // Noncompliant
            {
            }
        }

        private static void BackPropagation(object a, object b)
        {
            if (a == b && b == null)
            {
                a.ToString();
            }
        }

        public void RefEqualsImpliesValueEquals(object a, object b)
        {
            if (object.ReferenceEquals(a, b))
            {
                if (object.Equals(a, b)) { }    // Noncompliant {{Change this condition so that it does not always evaluate to "true".}}
                if (Equals(a, b)) { }           // Noncompliant {{Change this condition so that it does not always evaluate to "true".}}
                if (a.Equals(b)) { }            // Noncompliant {{Change this condition so that it does not always evaluate to "true".}}
            }

            if (this == a)
            {
                if (this.Equals(a)) { } // Noncompliant
                if (Equals(a)) { }      // Noncompliant
            }
        }

        public void ValueEqualsDoesNotImplyRefEquals(object a, object b)
        {
            if (object.Equals(a, b)) // 'a' could override Equals, so this is not a ref equality check
            {
                if (a == b) { } // Compliant
            }
        }

        public void ReferenceEqualsMethodCalls(object a, object b)
        {
            if (object.ReferenceEquals(a, b))
            {
                if (a == b) { } // Noncompliant {{Change this condition so that it does not always evaluate to "true".}}
            }

            if (a == b)
            {
                if (object.ReferenceEquals(a, b)) { } // Noncompliant {{Change this condition so that it does not always evaluate to "true".}}
            }
        }

        public void ReferenceEqualsMethodCallWithOpOverload(ConditionEvaluatesToConstant a, ConditionEvaluatesToConstant b)
        {
            if (object.ReferenceEquals(a, b))
            {
                if (a == b) { } // Noncompliant {{Change this condition so that it does not always evaluate to "true".}}
            }

            if (a == b)
            {
                if (object.ReferenceEquals(a, b)) { } // Compliant, == is doing a value comparison above.
            }
        }

        public void ReferenceEquals(object a, object b)
        {
            if (object.ReferenceEquals(a, b)) { }

            if (object.ReferenceEquals(a, a)) { } // Noncompliant {{Change this condition so that it does not always evaluate to "true".}}

            a = null;
            if (object.ReferenceEquals(null, a)) { } // Noncompliant {{Change this condition so that it does not always evaluate to "true".}}

            if (object.ReferenceEquals(null, new object())) { } // Noncompliant {{Change this condition so that it does not always evaluate to "false".}}

            int i = 10;
            if (object.ReferenceEquals(i, i)) { } // Noncompliant because of boxing {{Change this condition so that it does not always evaluate to "false".}}

            int? ii = null;
            int? jj = null;
            if (object.ReferenceEquals(ii, jj)) { } // Noncompliant {{Change this condition so that it does not always evaluate to "true".}}

            jj = 10;
            if (object.ReferenceEquals(ii, jj)) { } // Noncompliant {{Change this condition so that it does not always evaluate to "false".}}
        }

        public void ReferenceEqualsBool(bool a, bool b)
        {
            if (object.ReferenceEquals(a, b)) { } // Noncompliant {{Change this condition so that it does not always evaluate to "false".}}
        }

        public void ReferenceEqualsNullable(int? ii, int? jj)
        {
            if (object.ReferenceEquals(ii, jj)) { } // Compliant, they might be both null
            jj = 1;
            if (object.ReferenceEquals(ii, jj)) { } // Noncompliant {{Change this condition so that it does not always evaluate to "false".}}
        }

        public override bool Equals(object obj)
        {
            return base.Equals(obj);
        }

        public static bool operator ==(ConditionEvaluatesToConstant a, ConditionEvaluatesToConstant b)
        {
            return false;
        }

        public void StringEmpty()
        {
            string s = null;
            if (string.IsNullOrEmpty(s)) { } // Noncompliant {{Change this condition so that it does not always evaluate to "true".}}
            if (string.IsNullOrWhiteSpace(s)) { } // Noncompliant {{Change this condition so that it does not always evaluate to "true".}}
            if (string.IsInterned(s)) { }
            s = "";
            if (string.IsNullOrEmpty(s)) { }
            if (string.IsNullOrWhiteSpace(s)) { }
        }

        public void Comparisons(int i, int j)
        {
            if (i < j)
            {
                if (j < i) { }  // Noncompliant {{Change this condition so that it does not always evaluate to "false".}}
                if (j <= i) { } // Noncompliant {{Change this condition so that it does not always evaluate to "false".}}
                if (j == i) { } // Noncompliant {{Change this condition so that it does not always evaluate to "false".}}
                if (j != i) { } // Noncompliant {{Change this condition so that it does not always evaluate to "true".}}
            }

            if (i <= j)
            {
                if (j < i) { }  // Noncompliant {{Change this condition so that it does not always evaluate to "false".}}
                if (j <= i)
                {
                    if (j == i) { } // Noncompliant {{Change this condition so that it does not always evaluate to "true".}}
                    if (j != i) { } // Noncompliant {{Change this condition so that it does not always evaluate to "false".}}
                }
                if (j == i)
                {
                    if (i >= j) { } // Noncompliant {{Change this condition so that it does not always evaluate to "true".}}
                }
                if (j != i)
                {
                    if (i >= j) { } // Noncompliant {{Change this condition so that it does not always evaluate to "false".}}
                }
            }
        }

        int ValueEquals(int i, int j)
        {
            if (i == j)
            {
                if (Equals(i, j)) { } // Noncompliant
                if (i.Equals(j)) { }  // Noncompliant
            }
        }

        void DefaultExpression(object o)
        {
            if (default(o) == null) { } // Compliant
        }

        void ConditionalAccessNullPropagation(object o)
        {
            if (o == null)
            {
                if (o?.ToString() == null) { } // Noncompliant {{Change this condition so that it does not always evaluate to "true".}}
                if (o?.GetHashCode() == null) { } // Noncompliant {{Change this condition so that it does not always evaluate to "true".}}
            }
        }

        void Cast()
        {
            var i = 5;
            var o = (object)i;
            if (o == null) { } // Noncompliant {{Change this condition so that it does not always evaluate to "false".}}

            var x = (ConditionEvaluatesToConstant)o; // This would throw and invalid cast exception
            if (x == null) { } // Noncompliant {{Change this condition so that it does not always evaluate to "false".}}
        }

        public async Task NotNullAfterAccess(object o, int[,] arr, IEnumerable<int> coll, Task task)
        {
            Console.WriteLine(o.ToString());
            if (o == null) { } // Noncompliant {{Change this condition so that it does not always evaluate to "false".}}

            Console.WriteLine(arr[42, 42]);
            if (arr == null) { } // Noncompliant {{Change this condition so that it does not always evaluate to "false".}}

            foreach (var item in coll)
            {
            }
            if (coll == null) { } // Noncompliant {{Change this condition so that it does not always evaluate to "false".}}

            await task;
            if (task == null) { } // Noncompliant {{Change this condition so that it does not always evaluate to "false".}}
        }

        public void EnumMemberAccess()
        {
            var m = new MyClass();
            Console.WriteLine(m.myEnum);
            m = null;
            if (m?.myEnum == MyEnum.One) // Noncompliant {{Change this condition so that it does not always evaluate to "false".}}
            {
            }
        }

        int field;
        int GetValue() { return 42; }
        public void NullabiltyTest()
        {
            if (field == null)  // Noncompliant
            {
            }

            int i = GetValue();
            if (i == null)      // Noncompliant
            {

            }
        }

        public void EqualsTest32(object o)
        {
            var o2 = o;
            if (o == o2) { } // Noncompliant
            if (object.ReferenceEquals(o, o2)) { } // Noncompliant
            if (o.Equals(o2)) { } // Noncompliant
            if (object.Equals(o2, o)) { } // Noncompliant


            int i = 1;
            int j = i;
            if (i == j) // Noncompliant
            {
            }

            if (i.Equals(j)) { } // Noncompliant
            if (object.Equals(i, j)) { } // Noncompliant
        }

        async Task Test_Await_Constraint(object o, Task t)
        {
            if (t != null)
            {
                var o = await t;
                if (o == null) { } // Compliant, might be null
            }
        }

        enum MyEnum
        {
            One, Two
        }

        class MyClass
        {
            public MyEnum myEnum;
        }

        public void Assert(bool condition, object o1)
        {
            Debug.Assert(condition);

            if (condition) // Noncompliant
            {
            }

            Trace.Assert(condition); // Noncompliant
//                       ^^^^^^^^^

            if (o1 != null)
            {
                Debug.Assert(o1 == null, "Some message", "More details", 1, 2, 3); // Noncompliant
//                           ^^^^^^^^^^
            }
        }

        public void Assert(object o1)
        {
            System.Diagnostics.Debug.Assert(o1 != null);
            System.Diagnostics.Debug.Assert(o1 == null); // Noncompliant
        }

        void ComparisonTransitivity(int a, int b, int c)
        {
            if (a == b && b < c)
            {
                if (a >= c) { } // Noncompliant
            }
            if (a == b && b <= c)
            {
                if (a > c) { } // Noncompliant
            }
            if (a > b && b > c)
            {
                if (a <= c) { } // Noncompliant
            }
            if (a > b && b >= c)
            {
                if (a <= c) { } // Noncompliant
            }
            if (a >= b && b >= c)
            {
                if (a < c) { } // Noncompliant
            }
            if (a >= b && c <= b)
            {
                if (a < c) { } // Noncompliant
            }
            if (a >= b && c >= b)
            {
                if (a < c) { }
            }
        }

        void RefEqTransitivity(Comp a, Comp b, Comp c)
        {
            if (a == b && b == c)
            {
                if (a != c) { } // Noncompliant
            }
            if (a.Equals(b) && b == c)
            {
                if (a != c) { }
                if (a == c) { }
                if (a.Equals(c)) { }  // Noncompliant
                if (!a.Equals(c)) { } // Noncompliant
            }
            if (a > b && b == c)
            {
                if (a <= c) { } // Noncompliant
            }
        }

        void ValueEqTransitivity(Comp a, Comp b, Comp c)
        {
            if (a == b && b.Equals(c))
            {
                if (a.Equals(c)) { } // Noncompliant
            }
            if (a.Equals(b) && b.Equals(c))
            {
                if (a != c) { }
                if (a == c) { }
                if (a.Equals(c)) { }  // Noncompliant
                if (!a.Equals(c)) { } // Noncompliant
            }
            if (a > b && b.Equals(c))
            {
                if (a > c) { } // Noncompliant
                if (a <= c) { } // Noncompliant
            }
            if (!a.Equals(b) && b.Equals(c))
            {
                if (a.Equals(c)) { } // Noncompliant
                if (a == c) { } // Noncompliant
            }
            if (a != b && b.Equals(c))
            {
                if (a.Equals(c)) { }
                if (a == c) { }
            }
        }

        void NeqEqTransitivity(object a, object b, object c)
        {
            if (a == c && a != b)
            {
                if (b == c) { } // Noncompliant
                if (b.Equals(c)) { }
            }

            if (a == c && !a.Equals(b))
            {
                if (b == c) { }         // Noncompliant
                if (b.Equals(c)) { }    // Noncompliant
            }
        }

        class Comp
        {
            public static bool operator <(Comp a, Comp b) { return true; }
            public static bool operator >(Comp a, Comp b) { return true; }
            public static bool operator >=(Comp a, Comp b) { return true; }
            public static bool operator <=(Comp a, Comp b) { return true; }
        }

        struct MyStructWithNoOperator
        {
            public static void M(MyStructWithNoOperator a)
            {
                if (a == null) // Noncompliant, also a compiler error
                {
                }
            }
        }

        struct MyStructWithOperator
        {
            public static bool operator==(MyStructWithOperator? a, MyStructWithOperator? b)
            {
                return true;
            }

            public static bool operator !=(MyStructWithOperator? a, MyStructWithOperator? b)
            {
                return true;
            }

            public static void M(MyStructWithOperator a)
            {
                if (a == null) // Compliant
                {
                }
            }
        }
    }
}
