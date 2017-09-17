using System;
using KitchenResponsible.Utils.DateAndTime;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace KitchenResponsible.Filters {
    public class LessPoorButStillPoorCacheResourceFilterAttribute : Attribute, IResourceFilter {
        private static readonly IWeekNumberFinder weekNumberFinder;
        private static ViewResult cachedView;
        private static ushort previousWeek;

        static LessPoorButStillPoorCacheResourceFilterAttribute() {
            weekNumberFinder = new WeekNumberFinder();
        }

        public LessPoorButStillPoorCacheResourceFilterAttribute() {
            previousWeek = weekNumberFinder.GetIso8601WeekOfYear();
        }
        
        public void OnResourceExecuting(ResourceExecutingContext context)
        {
            // TODO: Hva med de andre nettsidene???
            if (context.HttpContext.Request.Path.ToString() != "/") {
                return;
            }

            if (cachedView == null) {
                return;
            }
            
            if (previousWeek != weekNumberFinder.GetIso8601WeekOfYear()) {
                cachedView = null;
            }

            context.Result = cachedView;
        }

        public void OnResourceExecuted(ResourceExecutedContext context) => 
            cachedView = context.Result as ViewResult;
    }
}