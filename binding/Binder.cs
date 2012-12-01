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

using System;
using SharpKit.JavaScript;
using guice.reflection;

namespace guice.binding {

    [JsType(JsMode.Prototype, OmitCasts = true, Export=false, Name = "Object")]
    public class BindingHashMap : JsObject<AbstractBinding> {
    }

    public class Binder {
        readonly BindingHashMap hashMap;

        public AbstractBinding getBinding( TypeDefinition typeDefinition ) {
            return hashMap[typeDefinition.getClassName()];
        }

        public void addBinding( AbstractBinding abstractBinding ) {
            hashMap[abstractBinding.getTypeName()] = abstractBinding;
        }

        public BindingFactory bind( Type type ) {
            var typeDefinition = new TypeDefinition(type);
            AbstractBinding existingBinding = getBinding( typeDefinition );

            //Do we already have a binding for this type?
            if (existingBinding != null) {
                /** Having a binding is actually not a problem, in most cases we accept that the last configured binding is the appropriate one
                 * However, in the case of a Singleton, this could actually wreak some havoc on the system, especially if this is a child injector situation and we now
                 * have multiple instances of a singleton..... SO, we throw an error if someone attempts to reconfigure a singleton. Incidentally, if you want your 
                 * parent and child injectors to be able to override 'global-ish' singlteon like objects, use the Context scope or make your own object and use the 
                 * instance binding. 
                 **/
                if ( existingBinding.getScope() == Scope.Singleton ) {
                    throw new JsError("Overriding bindings for Singleton Scoped injections is not allowed.");
                }
            }

            var factory = new BindingFactory(this, typeDefinition );
            return factory;
        }

        public Binder( BindingHashMap hashMap ) {
            this.hashMap = hashMap;
        }
    }
}