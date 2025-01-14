﻿/*
   Copyright 2011 - 2019 Adrian Popescu.

   Licensed under the Apache License, Version 2.0 (the "License");
   you may not use this file except in compliance with the License.
   You may obtain a copy of the License at

       http://www.apache.org/licenses/LICENSE-2.0

   Unless required by applicable law or agreed to in writing, software
   distributed under the License is distributed on an "AS IS" BASIS,
   WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
   See the License for the specific language governing permissions and
   limitations under the License.
*/

using System;
using System.Collections.Generic;
using System.Web.Script.Serialization;
using Redmine.Net.Api.Types;
using Redmine.Net.Api.Extensions;

namespace Redmine.Net.Api.JSonConverters
{
    internal class ProjectConverter : JavaScriptConverter
    {
        #region Overrides of JavaScriptConverter

        /// <summary>
        ///     When overridden in a derived class, converts the provided dictionary into an object of the specified type.
        /// </summary>
        /// <param name="dictionary">
        ///     An <see cref="T:System.Collections.Generic.IDictionary`2" /> instance of property data stored
        ///     as name/value pairs.
        /// </param>
        /// <param name="type">The type of the resulting object.</param>
        /// <param name="serializer">The <see cref="T:System.Web.Script.Serialization.JavaScriptSerializer" /> instance.</param>
        /// <returns>
        ///     The deserialized object.
        /// </returns>
        public override object Deserialize(IDictionary<string, object> dictionary, Type type,
            JavaScriptSerializer serializer)
        {
            if (dictionary != null)
            {
                var project = new Project();

                project.Id = dictionary.GetValue<int>(RedmineKeys.ID);
                project.Description = dictionary.GetValue<string>(RedmineKeys.DESCRIPTION);
                project.HomePage = dictionary.GetValue<string>(RedmineKeys.HOMEPAGE);
                project.Name = dictionary.GetValue<string>(RedmineKeys.NAME);
                project.Identifier = dictionary.GetValue<string>(RedmineKeys.IDENTIFIER);
                project.Status = dictionary.GetValue<ProjectStatus>(RedmineKeys.STATUS);
                project.CreatedOn = dictionary.GetValue<DateTime?>(RedmineKeys.CREATED_ON);
                project.UpdatedOn = dictionary.GetValue<DateTime?>(RedmineKeys.UPDATED_ON);
                project.Trackers = dictionary.GetValueAsCollection<ProjectTracker>(RedmineKeys.TRACKERS);
                project.CustomFields = dictionary.GetValueAsCollection<IssueCustomField>(RedmineKeys.CUSTOM_FIELDS);
                project.IsPublic = dictionary.GetValue<bool>(RedmineKeys.IS_PUBLIC);
                project.Parent = dictionary.GetValueAsIdentifiableName(RedmineKeys.PARENT);
                project.IssueCategories = dictionary.GetValueAsCollection<ProjectIssueCategory>(RedmineKeys.ISSUE_CATEGORIES);
                project.EnabledModules = dictionary.GetValueAsCollection<ProjectEnabledModule>(RedmineKeys.ENABLED_MODULES);
                project.TimeEntryActivities = dictionary.GetValueAsCollection<TimeEntryActivity>(RedmineKeys.TIME_ENTRY_ACTIVITIES);
                return project;
            }

            return null;
        }

        /// <summary>
        ///     When overridden in a derived class, builds a dictionary of name/value pairs.
        /// </summary>
        /// <param name="obj">The object to serialize.</param>
        /// <param name="serializer">The object that is responsible for the serialization.</param>
        /// <returns>
        ///     An object that contains key/value pairs that represent the object’s data.
        /// </returns>
        public override IDictionary<string, object> Serialize(object obj, JavaScriptSerializer serializer)
        {
            var entity = obj as Project;
            var result = new Dictionary<string, object>();

            if (entity != null)
            {
                result.Add(RedmineKeys.NAME, entity.Name);
                result.Add(RedmineKeys.IDENTIFIER, entity.Identifier);
                result.Add(RedmineKeys.DESCRIPTION, entity.Description);
                result.Add(RedmineKeys.HOMEPAGE, entity.HomePage);
                //result.Add(RedmineKeys.INHERIT_MEMBERS, entity.InheritMembers.ToString().ToLowerInvariant());
                result.Add(RedmineKeys.IS_PUBLIC, entity.IsPublic.ToString().ToLowerInvariant());
                result.WriteIdOrEmpty(entity.Parent, RedmineKeys.PARENT_ID, string.Empty);
                result.WriteIdsArray(RedmineKeys.TRACKER_IDS, entity.Trackers);
                result.WriteNamesArray(RedmineKeys.ENABLED_MODULE_NAMES, entity.EnabledModules);
                if (entity.Id > 0)
                {
                    result.WriteArray(RedmineKeys.CUSTOM_FIELDS, entity.CustomFields, new IssueCustomFieldConverter(),
                        serializer);
                }
                var root = new Dictionary<string, object>();
                root[RedmineKeys.PROJECT] = result;
                return root;
            }

            return result;
        }

        /// <summary>
        ///     When overridden in a derived class, gets a collection of the supported types.
        /// </summary>
        public override IEnumerable<Type> SupportedTypes
        {
            get { return new List<Type>(new[] { typeof(Project) }); }
        }

        #endregion
    }
}
