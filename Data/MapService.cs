using Aspose.Gis;
using Aspose.Gis.Rendering;
using Aspose.Gis.Rendering.Symbolizers;
using Microsoft.AspNetCore.Components;
using System;
using System.Collections.Generic;
using System.Xml;

namespace Geo.Svg.Map.Data
{
    public class MapService
    {
        public PolygonViewModel[] GetPolygons()
        {
            List<PolygonViewModel> polygons = new List<PolygonViewModel>();

            var layer = Drivers.GeoJson.OpenLayer("Files/polygons.json");            
            foreach (var item in layer)
            {
                var mapSize = 100;
                var itemLayer = Drivers.InMemory.CreateLayer();
                var feature = itemLayer.ConstructFeature();
                feature.Geometry = item.Geometry;
                itemLayer.Add(feature);

                string fileName = Guid.NewGuid().ToString("d");
                using (var map = new Aspose.Gis.Rendering.Map(mapSize, mapSize))
                {
                    map.Add(itemLayer);
                    map.Render(fileName, Renderers.Svg);
                }

                XmlDocument xDoc = new XmlDocument();
                xDoc.Load(fileName);
                XmlNode node = xDoc.ChildNodes[1].ChildNodes[0];
                var geometry = node.InnerXml;

                polygons.Add(new PolygonViewModel
                {
                    ItemNumber = polygons.Count,
                    SvgGeometry = (MarkupString) geometry
                });
            }
            
            return polygons.ToArray();
        }
    }
}

