
using PagedList.Mvc;

namespace WebUi.Helpers
{
    public static class PageListRenderOptionsHelper
    {
        public static PagedListRenderOptions MessagesPageListRenderOptions
        {
            get
            {
                return new PagedListRenderOptions()
                {
                    DisplayLinkToFirstPage = PagedListDisplayMode.Never,
                    DisplayLinkToLastPage = PagedListDisplayMode.Never,
                    DisplayLinkToPreviousPage = PagedListDisplayMode.Always,
                    DisplayLinkToNextPage = PagedListDisplayMode.Always,
                    DisplayLinkToIndividualPages = false,
                    UlElementClasses = new string[1]{"pager"},
                    ClassToApplyToFirstListItemInPager = "previous",
                    ClassToApplyToLastListItemInPager = "next",
                    LinkToPreviousPageFormat = "&larr; Newer",
                    LinkToNextPageFormat = "Older &rarr;"
                };
            }
        }

    }
}