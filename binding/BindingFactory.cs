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
using guice.binding.decorator;
using guice.reflection;

namespace guice.binding {

    /** Instance Scope mean it is an instance scope with no Guice rules governing the recreation of the object. So, Type & Provider bindings will be executed as requested.
     *  Instance bindings will always return the instance you configured, but you can reconfigure the instance binding in other contexts should you like.
     *  
     *  Singleton scope guarantees that Guice will only provide a single instance of executed binding for the entire system
     *  
     *  Context scope guarantees that Guice will only provide a single instance of the executed binding within the Context, however, other contexts can redefine the binding. 
     *  If your context does not define a binding, guicejs will inquire with parent contexts but not siblings.
     **/
    [JsType(JsMode.Json)]
    public enum Scope { Instance, Singleton, Context };

    public abstract class AbstractBinding {
        public abstract object provide(Injector injector);
        public abstract JsString getTypeName();
        public abstract Scope getScope();
    }

    public class BindingFactory {
        readonly Binder binder;
        readonly TypeDefinition typeDefinition;

        Scope scope;

        public AbstractBinding to(Type dependency) {
            AbstractBinding abstractBinding = withDecoration( new TypeAbstractBinding( typeDefinition, new TypeDefinition(dependency) ) );

            binder.addBinding(abstractBinding);
            return abstractBinding;
        }

        public AbstractBinding toInstance( object instance ) {
            //At first it seems silly to have a singleton decorator around an instance, but it affects our rules for overriding in ChildInjectors, so keep it
            AbstractBinding abstractBinding = withDecoration( new InstanceAbstractBinding( typeDefinition, instance ) );

            binder.addBinding(abstractBinding);
            return abstractBinding;
        }

        public AbstractBinding toProvider( Type providerType ) {
            AbstractBinding abstractBinding = withDecoration( new ProviderAbstractBinding(typeDefinition, new TypeDefinition(providerType) ) );
            binder.addBinding(abstractBinding);
            return abstractBinding;
        }

        public BindingFactory inScope( Scope scope ) {
            this.scope = scope;
            return this;
        }

        AbstractBinding withDecoration( AbstractBinding abstractBinding ) {
            if (scope == Scope.Singleton) {
                abstractBinding = new SingletonDecorator(abstractBinding);
            } else if (scope == Scope.Context ) {
                abstractBinding = new SingletonDecorator(abstractBinding);
            }

            return abstractBinding;
        }

        public BindingFactory(Binder binder, TypeDefinition typeDefinition) {
            this.binder = binder;
            this.typeDefinition = typeDefinition;
        }
    }
}