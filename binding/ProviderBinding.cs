﻿/***
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
using SharpKit.JavaScript;
using guice.reflection;

namespace guice.binding {

    public abstract class Provider {
        public abstract object get();
    }

    public class ProviderBinding : Binding {

        readonly TypeDefinition typeDefinition;
        readonly TypeDefinition providerTypeDefinition;

        private Provider provider;

        override public JsString getTypeName() {
            return typeDefinition.getClassName();
        }

        override public object provide(Injector injector) {

            if ( provider == null ) {
                provider = (Provider)injector.getInstance(providerTypeDefinition);
            }

            return provider.get();
        }

        public ProviderBinding(TypeDefinition typeDefinition, TypeDefinition providerTypeDefinition) {
            this.typeDefinition = typeDefinition;
            this.providerTypeDefinition = providerTypeDefinition;
        }
    }
}
