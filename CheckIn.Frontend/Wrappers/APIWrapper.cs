using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CheckIn.Frontend.Wrappers
{
    public class APIWrapper<T>
    {
        /// <summary>
        /// Cotains result from the request
        /// </summary>
        public T Result { get; set; }
        /// <summary>
        /// Check if the request was succesful
        /// </summary>
        public bool HasSucceded { get; private set; }
        /// <summary>
        /// Contains a list of errors
        /// </summary>
        public string[] Errors { get; private set; }
        /// <summary>
        /// Instanciate a success wrapper
        /// </summary>
        /// <param name="Data"></param>
        public APIWrapper(T Data)
        {
            Result = Data;
            HasSucceded = true;
        }
        /// <summary>
        /// Instansiate a failiure wrapper
        /// </summary>
        /// <param name="Data"></param>
        /// <param name="Errors"></param>
        public APIWrapper(T Data, params string[] Errors)
        {
            Result = Data;
            this.Errors = Errors;
            HasSucceded = false;
        }


    }
}
