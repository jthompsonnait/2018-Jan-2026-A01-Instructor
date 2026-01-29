<Query Kind="Statements" />

List<int> numbersA = [1,2,3,4];
List<int> numbersB = [3,4,5,6];

numbersA.Union(numbersB).Dump("Union");
numbersB.Union(numbersA).Dump("Union");
numbersA.Intersect(numbersB).Dump("Intersect");
numbersA.Except(numbersB).Dump("Except");
numbersA.Concat(numbersB).Dump("Concat");