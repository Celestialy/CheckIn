using CheckIn.Frontend.Wrappers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CheckIn.Frontend.Services
{
    public abstract class BaseService
    {
        /// <summary>
        /// Wraps data in a success wrapper
        /// </summary>
        /// <typeparam name="T">Wrapping type</typeparam>
        /// <param name="data">Gets wrapped</param>
        /// <returns></returns>
        public virtual APIWrapper<T> Data<T>(T data)
        {
            return new APIWrapper<T>(data);
        }
        /// <summary>
        /// Wraps result in a failure wrapper
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="data"></param>
        /// <param name="Errors">Errors</param>
        /// <returns></returns>
        public virtual APIWrapper<T> Error<T>(T data,  params string[] Errors)
        {
            return new APIWrapper<T>(data, Errors);
        }
    }
}
