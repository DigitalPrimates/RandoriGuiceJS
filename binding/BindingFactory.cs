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

    [JsType(JsMode.Json)]
    public enum Scope { Instance, Singleton };

    public abstract class Binding {
        public abstract object provide(Injector injector);
        public abstract JsString getTypeName();
    }

    public class BindingFactory {
        readonly Binder binder;
        readonly TypeDefinition typeDefinition;

        Scope scope;

        public Binding to(Type dependency) {
            Binding binding = withDecoration( new TypeBinding( typeDefinition, new TypeDefinition(dependency) ) );

            binder.addBinding(binding);
            return binding;
        }

        public Binding toInstance( object instance ) {
            Binding binding = new InstanceBinding( typeDefinition, instance );

            binder.addBinding(binding);
            return binding;
        }

        public Binding toProvider( Type providerType ) {
            Binding binding = withDecoration( new ProviderBinding(typeDefinition, new TypeDefinition(providerType) ) );
            binder.addBinding(binding);
            return binding;
        }

        public BindingFactory inScope( Scope scope ) {
            this.scope = scope;
            return this;
        }

        Binding withDecoration( Binding binding ) {
            if (scope == Scope.Singleton) {
                binding = new SingletonDecorator(binding);
            }

            return binding;
        }

        public BindingFactory(Binder binder, TypeDefinition typeDefinition) {
            this.binder = binder;
            this.typeDefinition = typeDefinition;
        }
    }
}