using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PagedList;

namespace SocialNetwork.Bll.Interface
{
    public interface IMappedPagedList<out T> : IPagedList<T>
    {
        /// <summary>
        /// Project an element on page into new form.
        /// </summary>
        /// <typeparam name="TRes">type resulted PageList items</typeparam>
        /// <param name="convert">function to convert</param>
        /// <returns></returns>
        IMappedPagedList<TRes> Map<TRes>(Func<T, TRes> convert);
    }
}
