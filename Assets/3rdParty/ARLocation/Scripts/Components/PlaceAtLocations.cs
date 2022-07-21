using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;
using KleinEngine;
using AppLogic;

// ReSharper disable CollectionNeverQueried.Local
// ReSharper disable MemberCanBePrivate.Global

namespace ARLocation
{

    /// <summary>
    /// This class instantiates a prefab at the given GPS locations. Must
    /// be in the `ARLocationRoot` GameObject with a `ARLocatedObjectsManager`
    /// Component.
    /// </summary>
    [AddComponentMenu("AR+GPS/Place At Locations")]
    [HelpURL("https://http://docs.unity-ar-gps-location.com/guide/#placeatlocations")]
    public class PlaceAtLocations : MonoBehaviour
    {
        [Serializable]
        public class Entry
        {
            public LocationData ObjectLocation;
            public OverrideAltitudeData OverrideAltitude = new OverrideAltitudeData();
        }

        [Tooltip("The locations where the objects will be instantiated.")]
        public List<PlaceAtLocation.LocationSettingsData> Locations;

        public PlaceAtLocation.PlaceAtOptions PlacementOptions;

        /// <summary>
        /// The game object that will be instantiated.
        /// </summary>
        [FormerlySerializedAs("prefab")] [Tooltip("The game object that will be instantiated.")]
        public GameObject Prefab;

        [Space(4.0f)]

        [Header("Debug")]
        [Tooltip("When debug mode is enabled, this component will print relevant messages to the console. Filter by 'PlateAtLocations' in the log output to see the messages.")]
        public bool DebugMode;

        [Space(4.0f)]

        private  List<Location> locations = new List<Location>();
        private  List<GameObject> instances = new List<GameObject>();

        public List<GameObject> Instances => instances;

        private void Awake()
        {
            AppFacade.GetInstance().addEvent(ModuleEventType.GET_POI_POS_IN_UNITY, HandleGetPOIInUnity);
        }

        private void HandleGetPOIInUnity(EventObject ev)
        {
            Debug.Log("HandleGetPOIInUnity...");
            WayzPOI wayzPOI = ev.param as WayzPOI;
            if (wayzPOI == null) return;
            Location locationTemp = new Location();
            double[] currentGps = GPSUtil.gcj02_To_Gps84(wayzPOI.currentLocation.lat, wayzPOI.currentLocation.lon);//高德获取为火星坐标系 gcj02 转换一下
            locationTemp.Latitude = currentGps[0];
            locationTemp.Longitude = currentGps[1];

            //locationTemp.Latitude = wayzPOI.currentLocation.lat;
            //locationTemp.Longitude = wayzPOI.currentLocation.lon;

            AIMapPOICommand.currentLocation = locationTemp;
            if (wayzPOI.POIList == null) return;
            Debug.Log(wayzPOI.POIList.Count);
            foreach (var poi in wayzPOI.POIList)
            {
                Location data = new Location();
                data.Label = poi.name;
                double[] gps = GPSUtil.gcj02_To_Gps84(poi.lat, poi.lon);

                data.Latitude = gps[0];
                data.Longitude = gps[1];
                data.Altitude = 0;
                locations.Add(data);
            }


            foreach (Location location in locations)
            {
                AddLocation(location);
            }
        }

        public void AddLocation(Location location)
        {
            var instance = PlaceAtLocation.CreatePlacedInstance(Prefab, location, PlacementOptions, DebugMode);

            instance.name = $"{gameObject.name} - {locations.Count}";

            //locations.Add(location);
            instances.Add(instance);
        }

        private void Start()
        {
            //foreach (var entry in Locations)
            //{
            //    var newLoc = entry.GetLocation();

            //    AddLocation(newLoc);
            //}
        }

        //public void AddLocation(Location location)
        //{
        //    var instance = PlaceAtLocation.CreatePlacedInstance(Prefab, location, PlacementOptions, DebugMode);

        //    instance.name = $"{gameObject.name} - {locations.Count}";

        //    locations.Add(location);
        //    instances.Add(instance);
        //}
    }
}
