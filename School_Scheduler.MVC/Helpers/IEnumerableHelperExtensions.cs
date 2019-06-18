using System;
using System.Collections.Generic;
using System.Linq;

namespace School_Scheduler.MVC.Helpers
{
    public static class IEnumerableHelperExtensions
    {
        public static T PickRandom<T>(this IEnumerable<T> items) where T : class => items?.OrderBy(item => Guid.NewGuid()).FirstOrDefault();
        public static T PickRandom<T>(this IEnumerable<T> items, Random rng) where T : class => items?.OrderBy(item => rng.Next()).FirstOrDefault();
    }
}