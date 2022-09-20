/*
 * * * * This bare-bones script was auto-generated * * * *
 * The code commented with "/ * * /" demonstrates how data is retrieved and passed to the adapter, plus other common commands. You can remove/replace it once you've got the idea
 * Complete it according to your specific use-case
 * Consult the Example scripts if you get stuck, as they provide solutions to most common scenarios
 * 
 * Main terms to understand:
 *		Model = class that contains the data associated with an item (title, content, icon etc.)
 *		Views Holder = class that contains references to your views (Text, Image, MonoBehavior, etc.)
 * 
 * Default expected UI hiererchy:
 *	  ...
 *		-Canvas
 *		  ...
 *			-MyScrollViewAdapter
 *				-Viewport
 *					-Content
 *				-Scrollbar (Optional)
 *				-ItemPrefab (Optional)
 * 
 * Note: If using Visual Studio and opening generated scripts for the first time, sometimes Intellisense (autocompletion)
 * won't work. This is a well-known bug and the solution is here: https://developercommunity.visualstudio.com/content/problem/130597/unity-intellisense-not-working-after-creating-new-1.html (or google "unity intellisense not working new script")
 * 
 * 
 * Please read the manual under "/Docs", as it contains everything you need to know in order to get started, including FAQ
 */

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using frame8.Logic.Misc.Other.Extensions;
using Com.TheFallenGames.OSA.CustomAdapters.GridView;
using Com.TheFallenGames.OSA.DataHelpers;
using Waku.Module;
using QFramework.UI;
using UniRx;
using zoomgame.Scripts.Architecture.TypeEvent;
using WeiXiang;

// You should modify the namespace to your own or - if you're sure there won't ever be conflicts - remove it altogether
namespace zoomgame
{
	// There is 1 important callback you need to implement, apart from Start(): UpdateCellViewsHolder()
	// See explanations below
	public class BasicGridAssestScorlPanel : GridAdapter<GridParams, MyGridItemViewsHolder>
	{
		public static BasicGridAssestScorlPanel ins;
		public int Num;
		private HttpDownLoad loader;
		private FloatReactiveProperty progress = new FloatReactiveProperty();
		 private Image progressSlider;
		public SimpleDataHelper<AssestInfoItemModel> Data { get; private set; }
		List<AssetInfoItem> evt1 = new List<AssetInfoItem>();
		UpdateServerListEvent evtOrigin;
		bool isFirst;
		public Text text;//测试

		#region GridAdapter implementation
		protected override void Awake()
        {
			base.Awake();
			ins = this;
        }
        protected override void Start()
		{
			Data = new SimpleDataHelper<AssestInfoItemModel>(this);

			// Calling this initializes internal data and prepares the adapter to handle item count changes
			base.Start();
			isFirst = true;
			// Retrieve the models from your data source and set the items count
			
			MessageBroker.Default.Receive<UpdateServerListEvent>().Subscribe(RetrieveDataAndUpdate).AddTo(this);
			
			//MessageBroker.Default.Receive<DownloadEvent>().Subscribe(OnDownload).AddTo(this);
			//         progress.Subscribe(v =>
			//         {
			//             if (progressSlider != null)
			//             {
			//		Debug.LogWarning("场景下载进度" + progress + "||" + v);
			//		progressSlider.fillAmount = progress;
			//             }

			//         });

		}

        // This is called anytime a previously invisible item become visible, or after it's created, 
        // or when anything that requires a refresh happens
        // Here you bind the data from the model to the item's views
        // *For the method's full description check the base implementation
        protected override void UpdateCellViewsHolder(MyGridItemViewsHolder newOrRecycled)
		{
			// In this callback, "newOrRecycled.ItemIndex" is guaranteed to always reflect the
			// index of item that should be represented by this views holder. You'll use this index
			// to retrieve the model from your data set
			
			AssestInfoItemModel model = Data[newOrRecycled.ItemIndex];
			newOrRecycled.LoadProcessImgBG.gameObject.SetActive(false);
			AssesInfoScorlItem item = newOrRecycled.BG.transform.GetComponent<AssesInfoScorlItem>();
			item.SetLoader(model.loader, model.progress);
			item.Init(model.assetInfoItemModel,model.isDownLoadIng, newOrRecycled.ItemIndex);
			
			//progressSlider = newOrRecycled.LoadProcessImg;
			
		}

		// This is the best place to clear an item's views in order to prepare it from being recycled, but this is not always needed, 
		// especially if the views' values are being overwritten anyway. Instead, this can be used to, for example, cancel an image 
		// download request, if it's still in progress when the item goes out of the viewport.
		// <newItemIndex> will be non-negative if this item will be recycled as opposed to just being disabled
		// *For the method's full description check the base implementation
		/*
		protected override void OnBeforeRecycleOrDisableCellViewsHolder(MyGridItemViewsHolder inRecycleBinOrVisible, int newItemIndex)
		{
			base.OnBeforeRecycleOrDisableCellViewsHolder(inRecycleBinOrVisible, newItemIndex);
		}
		*/
		#endregion

		// These are common data manipulation methods
		// The list containing the models is managed by you. The adapter only manages the items' sizes and the count
		// The adapter needs to be notified of any change that occurs in the data list. 
		// For GridAdapters, only Refresh and ResetItems work for now
		#region data manipulation
		public void AddItemsAt(int index, IList<AssestInfoItemModel> items)
		{
			//Commented: this only works with Lists. ATM, Insert for Grids only works by manually changing the list and calling NotifyListChangedExternally() after
			//Data.InsertItems(index, items);
			Data.List.InsertRange(index, items);
			Data.NotifyListChangedExternally();
		}

		public void RemoveItemsFrom(int index, int count)
		{
			//Commented: this only works with Lists. ATM, Remove for Grids only works by manually changing the list and calling NotifyListChangedExternally() after
			//Data.RemoveRange(index, count);
			Data.List.RemoveRange(index, count);
			Data.NotifyListChangedExternally();
		}

		public void SetItems(IList<AssestInfoItemModel> items)
		{
			Data.ResetItems(items);
		}
		#endregion

		public void UpdateView()
        {
			//if (evtOrigin != null)
			//{
			//	StartCoroutine(IEStartRetrieveDataAndUpdate(evtOrigin));
			//}
		}
		void showAll()
        {
            if (evtOrigin!=null)
            {
				RetrieveDataAndUpdate(evtOrigin);
			}
			

		}
		// Here, we're requesting <count> items from the data source
		public void RetrieveDataAndUpdate(UpdateServerListEvent evt=null)
		{
				evtOrigin = evt;
				StartCoroutine(IEStartRetrieveDataAndUpdate(evt));
			
		}
		IEnumerator IEStartRetrieveDataAndUpdate(UpdateServerListEvent evt = null)
		{
            yield return new WaitUntil(() =>
            {
                if (AmapLocation.Instance.IsRunning&&||AmapLocation.Instance.Accuracy<3||AmapLocation.Instance.Longitude!=0|| AmapLocation.Instance.Latitude!=0)
                {
					return true;
                }
				return false;
               // return AmapLocation.Instance.IsRunning;
            });
			text.text ="我的位置" +AmapLocation.Instance.Longitude + AmapLocation.Instance.Latitude;
            //TODO 判断是否在电子围栏里面
            List<AssetInfoItem> inAreaInfoList = new List<AssetInfoItem>();
			foreach (var item in evt.infoList)
			{
				List<Vector2> coordinatList = new List<Vector2>();
				//Debug.LogError("长度" + item.boundary.coordinates[0].Count);
				for (int i = 0; i < item.boundary.coordinates[0].Count; i++)
				{
					Vector2 vector2;
					vector2.x = item.boundary.coordinates[0][i][0];
					vector2.y = item.boundary.coordinates[0][i][1];
					coordinatList.Add(vector2);
				}
				//           foreach (var i in coordinatList)
				//           {
				//Debug.LogError("转换的经纬度"+i.x+i.y);
				//           }
				//if (AssetDownloader.Instance.IsPointInPolygon(new Vector2(121.603962f, 31.179835f), coordinatList))
				if (AssetDownloader.Instance.IsPointInPolygon(new Vector2(AmapLocation.Instance.Longitude, AmapLocation.Instance.Latitude), coordinatList))

				{
					inAreaInfoList.Add(item);
				}
			}
			// inAreaInfoList = evt.infoList;
			//List<AssetInfoItem> inAreaInfoList = evt1;

			if (evt1 == null)
			{
				evt1 = inAreaInfoList;
				isFirst = true;
			}
			else if (evt1 == evt.infoList)
			{
				isFirst = false;
			}
			else
			{
				evt1 = inAreaInfoList;
				isFirst = true;
			}


			int count = evt1.Count;
			if (count <= 0)
			{
				Debug.LogWarning("没有资源信息");
            }
            else
            {
				Num = count;

				StartCoroutine(FetchMoreItemsFromDataSourceAndUpdate(evt1, count));
			}
			yield return null;
		}
		// Retrieving <count> models from the data source and calling OnDataRetrieved after.
		// In a real case scenario, you'd query your server, your database or whatever is your data source and call OnDataRetrieved after
		IEnumerator FetchMoreItemsFromDataSourceAndUpdate(List<AssetInfoItem> evt,int count)
		{
			// Simulating data retrieving delay
			yield return new WaitForSeconds(.5f);
			
			var newItems = new AssestInfoItemModel[count];

			// Retrieve your data here
			Debug.LogError("总数"+count);
			for (int i = 0; i < count; i++)
			{
				var model = new AssestInfoItemModel()
				{
					assetInfoItemModel = evt[i],
					isDownLoadIng = false,
					loader = new HttpDownLoad(),
					progress = new FloatReactiveProperty(),
					isComplete = false,
					
				};
				newItems[i] = model;
			}
            foreach (var item in newItems)
            {
				Debug.LogError("驾驶科技的厚爱都爱"+item.isDownLoadIng);

            }
			OnDataRetrieved(newItems);
            //if (isFirst)
            //{
            //    OnDataRetrieved(newItems);
            //}

        }

		void OnDataRetrieved(AssestInfoItemModel[] newItems)
		{
			//Commented: this only works with Lists. ATM, Insert for Grids only works by manually changing the list and calling NotifyListChangedExternally() after
			// Data.InsertItemsAtEnd(newItems);

			Data.List.AddRange(newItems);
			Data.NotifyListChangedExternally();
		}
		/// <summary>
		/// 根据电子围栏进行筛选
		/// </summary>
		void SelectInfo(UpdateServerListEvent evt =null)
        {
			List<AssetInfoItem> inAreaInfoList = new List<AssetInfoItem>();
			foreach (var item in evt.infoList)
			{
				List<Vector2> coordinatList = new List<Vector2>();
				Debug.LogError("长度" + item.boundary.coordinates[0].Count);
				for (int i = 0; i < item.boundary.coordinates[0].Count; i++)
				{
					Vector2 vector2;
					vector2.x = item.boundary.coordinates[0][i][0];
					vector2.y = item.boundary.coordinates[0][i][1];
					coordinatList.Add(vector2);
				}
				//           foreach (var i in coordinatList)
				//           {
				//Debug.LogError("转换的经纬度"+i.x+i.y);
				//           }
				if (AssetDownloader.Instance.IsPointInPolygon(/*new Vector2(AmapLocation.Instance.Longitude, AmapLocation.Instance.Latitude)*/new Vector2(104.067768f, 30.552795f), coordinatList))
				{
					inAreaInfoList.Add(item);
				}
			}
			evt1 = inAreaInfoList;
			RetrieveDataAndUpdate();
		}

	}


	// Class containing the data associated with an item
	public class AssestInfoItemModel
	{

		public AssetInfoItem assetInfoItemModel;
		public bool isDownLoadIng;//是否正在下载中
		public HttpDownLoad loader;
		public FloatReactiveProperty progress = new FloatReactiveProperty();
		public bool isComplete;//是否下载好 
		public bool isNeedDown = false;
		public string nameTitle;
	}


	// This class keeps references to an item's views.
	// Your views holder should extend BaseItemViewsHolder for ListViews and CellViewsHolder for GridViews
	// The cell views holder should have a single child (usually named "Views"), which contains the actual 
	// UI elements. A cell's root is never disabled - when a cell is removed, only its "views" GameObject will be disabled
	public class MyGridItemViewsHolder : CellViewsHolder
	{
		
		public Text AssestInfoTxt;//资源title
		public Image BG;//背景图,未下载需要灰色
		public Image LoadProcessImg;//下载进度条
		public Transform LoadProcessImgBG;

		// Retrieving the views from the item's root GameObject
		public override void CollectViews()
		{
			base.CollectViews();

			// GetComponentAtPath is a handy extension method from frame8.Logic.Misc.Other.Extensions
			// which infers the variable's component from its type, so you won't need to specify it yourself
			
			views.GetComponentAtPath("AssestInfoTxt", out AssestInfoTxt);
			views.GetComponentAtPath("BG", out BG);
			views.GetComponentAtPath("LoadProcessImg", out LoadProcessImg);
			views.GetComponentAtPath("BG/LoadProcessImgBG", out LoadProcessImgBG);
		}
		
		// This is usually the only child of the item's root and it's called "Views". 
		// That's what the default implementation will look for, but just for flexibility, 
		// this callback is provided, in case it's named differently or there's more than 1 child 
		// *See GridExample.cs for more info
		/*
		protected override RectTransform GetViews()
		{ return root.Find("Views").transform as RectTransform; }
		*/

		// Override this if you have children layout groups. They need to be marked for rebuild when this callback is fired
		/*
		public override void MarkForRebuild()
		{
			base.MarkForRebuild();

			LayoutRebuilder.MarkLayoutForRebuild(yourChildLayout1);
			LayoutRebuilder.MarkLayoutForRebuild(yourChildLayout2);
			AChildSizeFitter.enabled = true;
		}
		*/

		// Override this if you've also overridden MarkForRebuild()
		/*
		public override void UnmarkForRebuild()
		{
			AChildSizeFitter.enabled = false;

			base.UnmarkForRebuild();
		}
		*/
	}
}
