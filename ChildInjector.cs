/***
 * Copyright 2012 LTN Consulting, Inc. /dba Digital Primates®
 * 
 * Licensed under the Apache License, Version 2.0 (the "License");
 * you may not use this file except in compliance with the License.
 * You may obtain a copy of the License at
 * 
 * http://www.apache.org/licenses/LICENSE-2.0
 * 
 * Unless required by applicable law or agreed to in writing, software
 * distributed under the License is distributed on an "AS IS" BASIS,
 * WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
 * See the License for the specific language governing permissions and
 * limitations under the License.
 * 
 * 
 * @author Michael Labriola <labriola@digitalprimates.net>
 */

using guice.binding;
using guice.reflection;
using guice.resolvers;

namespace guice {
    public class ChildInjector : Injector {
        readonly Injector parentInjector;

        internal override Binding getBinding(TypeDefinition typeDefinition) {
            //First we try to resolve it on our own, without own binding
            Binding binding = binder.getBinding(typeDefinition);

            //if we do not have a specific binding for it, we need to check to see if our parent injector has a specific binding for it before we just go building stuff
            if (binding == null) {
                binding = parentInjector.getBinding(typeDefinition);
            }

            return binding;
        }

        public ChildInjector(Binder binder, ClassResolver classResolver, Injector parentInjector)
            : base(binder, classResolver) {
            this.parentInjector = parentInjector;

            //Child injectors set themselves up as the new default Injector for the tree below them
            binder.bind(typeof(Injector)).toInstance(this);
        }
    }
}
