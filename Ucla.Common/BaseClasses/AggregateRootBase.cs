using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ucla.Common.BaseClasses
{
    /// <summary>
    /// Aggregate Root domain objects should be derive3d from this class.
    /// </summary>
    public class AggregateRootBase : DomainBase
    {
        public Boolean IsGraphDirty
        {
            get { return GetGraphDirty();  }
        }

        /// <summary>
        /// Method to determine if the object graph has any changes since it
        /// it was either fetched from the data store (or, if just created
        /// on the client, it should be true as well).
        /// </summary>
        /// The difference between the result of this method and just checking
        /// the IsDirty property of a domain object is that this method also
        /// checks to see if the root object or any of its children are dirty.
        /// The default implementation here works for degenerate aggregate roots
        /// with no children. Non-trivial aggregate root objects should override
        /// this method to return true if the root or any of its children are
        /// dirty.
        /// <returns>True if the domain object or any of its children have changes
        /// that would need to be saved to the data store.</returns>
        protected virtual bool GetGraphDirty()
        {
            return this.IsDirty;
        }

    }
}
