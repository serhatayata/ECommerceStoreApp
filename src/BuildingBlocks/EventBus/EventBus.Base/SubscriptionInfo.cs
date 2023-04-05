using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EventBus.Base
{
    public class SubscriptionInfo
    {
        public SubscriptionInfo(Type handlerType)
        {
            HandlerType = handlerType;
        }
        public Type HandlerType { get; set; }

        public static SubscriptionInfo Typed(Type handlerType)
        {
            return new SubscriptionInfo(handlerType);
        }
    }
}
