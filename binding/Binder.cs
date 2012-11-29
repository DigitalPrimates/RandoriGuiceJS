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

    [JsType(JsMode.Prototype, OmitCasts = true)]
    public class BindingHashMap : JsObject<Binding> {
    }

    public class Binder {
        readonly BindingHashMap hashMap;

        public Binding getBinding( TypeDefinition typeDefinition ) {
            return hashMap[typeDefinition.getClassName()];
        }

        public void addBinding( Binding binding ) {
            hashMap[binding.getTypeName()] = binding;
        }

        public BindingFactory bind( Type type ) {
            var typeDefinition = new TypeDefinition(type);
            var factory = new BindingFactory(this, typeDefinition );
            return factory;
        }

        public Binder( BindingHashMap hashMap ) {
            this.hashMap = hashMap;
        }
    }
}