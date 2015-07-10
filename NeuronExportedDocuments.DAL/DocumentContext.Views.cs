﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------
[assembly: System.Data.Entity.Infrastructure.MappingViews.DbMappingViewCacheTypeAttribute(typeof(NeuronExportedDocuments.DAL.DocumentContext), typeof(Edm_EntityMappingGeneratedViews.ViewsForBaseEntitySets62e67a7f087c4945bc0ab9ba130c8a8e))]
namespace Edm_EntityMappingGeneratedViews
{
	using System.Collections.Generic;
	using System.Data.Entity.Core.Metadata.Edm;
	using System.Data.Entity.Infrastructure.MappingViews;
	using System.Reflection;
	using System.Xml;
	using System.Xml.Linq;

	/// <Summary>
	/// The type contains views for EntitySets and AssociationSets that were generated at design time.
	/// </Summary>
	public sealed class ViewsForBaseEntitySets62e67a7f087c4945bc0ab9ba130c8a8e : DbMappingViewCache
	{
		private static Dictionary<string, DbMappingView> extentViews = null;
		private readonly static object lockObject = new object();

		public override string MappingHashValue { get { return "4a2becba28fa4390c68fab0ba7516e3ac72569d0ca4721257347cc61e2c64dd2"; } }

		/// <Summary>
		/// The constructor stores the views for the extents and also the hash values generated based on the metadata and mapping closure and views.
		/// </Summary>
		public ViewsForBaseEntitySets62e67a7f087c4945bc0ab9ba130c8a8e()
		{
		}

		/// <Summary>
		/// The method returns the view for the index given.
		/// </Summary>
		public override DbMappingView GetView(EntitySetBase extent)
		{
			// do not lock if views are loaded
			if (extentViews == null)
			{
				lock(lockObject)
				{
					if (extentViews == null)
					{
						LoadViews();
					}
				}
			}

			DbMappingView view;
			extentViews.TryGetValue(GetExtentFullName(extent), out view);
			return view;
		}

		private static void LoadViews()
		{
			extentViews = new Dictionary<string, DbMappingView>();

			using (var stream = Assembly.GetExecutingAssembly().GetManifestResourceStream("NeuronExportedDocuments.DAL.DocumentContext.Views.xml"))
			using (var reader = XmlReader.Create(stream))
			{
				while (reader.ReadToFollowing("view"))
				{
					extentViews.Add(reader["extent"], new DbMappingView(reader.ReadElementContentAsString()));
				}
			}
		}

		private static string GetExtentFullName(EntitySetBase entitySet)
		{
			return string.Format("{0}.{1}", entitySet.EntityContainer.Name, entitySet.Name);
		}
	}
}
