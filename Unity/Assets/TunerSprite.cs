using UnityEngine;
using System.Collections;

[RequireComponent(typeof(MeshFilter)), RequireComponent(typeof(MeshRenderer)), ExecuteInEditMode]
public class TunerSprite : MonoBehaviour
{

		// Use this for initialization
		void Start ()
		{
	
		}
			
		public void Init (Camera stage, float PX_Scale, float PX_Offset, float PY_Scale, float PY_Offset, float Width_Scale,
	                  float Width_Offset, float Height_Scale, float Height_Offset)
		{
				mStage = stage;
				mPX_Scale = PX_Scale;
				mPX_Offset = PX_Offset;
				mPY_Scale = PY_Scale;
				mPY_Offset = PY_Offset;
				mWidth_Scale = Width_Scale;
				mWidth_Offset = Width_Offset;
				mHeight_Scale = Height_Scale;
				mHeight_Offset = Height_Offset;
		}
		
		public int defaultParentWidth = 800;
		public int defaultParentHeight = 600;
		public Camera mStage = null;
		public float mPX_Scale = 0;
		public	float mPX_Offset = 0;
		public	float mPY_Scale = 0;
		public	float mPY_Offset = 0;
		public	float mWidth_Scale = 0;
		public	float mWidth_Offset = 0;
		public	float mHeight_Scale = 0;
		public	float mHeight_Offset = 0;
		TunerSprite mParent = null;

		public TunerSprite Parent {
				get {
						return mParent;
				}
				set {
						mParent = value;
				}
		}

		int Width {
				get {
						//calculate by parent.
						return	 (int)(ParentWidth * mWidth_Scale + mWidth_Offset);
				}
		}

		int Height {
				get {
						//calculate by parent.
						return	 (int)(ParentHeight * mHeight_Scale + mHeight_Offset);
				}
		}

		int PositionX {
				get {
						//calculate by parent.
						return	(int)(ParentPositionX + ParentWidth * mPX_Scale + mPX_Offset);
				}
		}

		int PositionY {
				get {
						//calculate by parent.
						return	(int)(ParentPositionY + ParentHeight * mPY_Scale + mPY_Offset);
	
				}
		}

		float PositionZ {
				get {
						//calculate by parent.
						return	ParentPositionZ;
			
				}
		}

		int ParentPositionX {
				get {

						//prent type.	1.sprite.	2.camera.	3.0
						int parentX;
						if (Parent != null) {
								parentX = Parent.PositionX;
						} else if (mStage != null) {
								parentX = (int)(mStage.transform.position.x - mStage.orthographicSize * mStage.aspect);
						} else {
								parentX = 0;
						}
			
						return parentX;
				}
		}

		int ParentPositionY {
				get {
						//prent type.	1.sprite.	2.camera.	3.0
						int parentY;
						if (Parent != null) {
								parentY = Parent.PositionY;
						} else if (mStage != null) {
								parentY = (int)(mStage.transform.position.y + mStage.orthographicSize);
						} else {
								parentY = 0;
						}
						return parentY;
				}
		}

		float ParentPositionZ {
				get {
						//prent type.	1.sprite.	2.camera.	3.0
						float parentZ;
						if (Parent != null) {
								parentZ = Parent.PositionZ;
						} else if (mStage != null) {
								parentZ = mStage.transform.position.z + 2;
						} else {
								parentZ = 0;
						}
			
						return parentZ;
				}
		}

		int ParentWidth {
				get {
						int parentWidth;
						
						//parent type. 1.sprite. 2.camera. 3.texture. 4.200
						if (Parent != null) {
								parentWidth = Parent.Width;
						} else if (mStage != null) {
								parentWidth = (int)(mStage.orthographicSize * 2 * mStage.aspect);
	
						} else if (renderer.sharedMaterial.mainTexture != null) {
								parentWidth = renderer.sharedMaterial.mainTexture.width;
						} else {
								parentWidth = 200;
						}

						return parentWidth;
				}
		}

		int ParentHeight {
				get {
						int parentHeight;
			
						//parent type. 1.sprite. 2.camera. 3.texture. 4.200
						if (Parent != null) {
								parentHeight = Parent.Height;
						} else if (mStage != null) {
			
								parentHeight = (int)mStage.orthographicSize * 2;

						} else if (renderer.sharedMaterial.mainTexture != null) {
								parentHeight = renderer.sharedMaterial.mainTexture.height;
						} else {
								parentHeight = 200;
						}
			
						return parentHeight;
				}
		}
		
		void RebuildMesh ()
		{
				Debug.Log (PositionX);
				MeshFilter mf = base.GetComponent (typeof(MeshFilter)) as MeshFilter;
				if (mf.sharedMesh != null) {
						UnityEngine.Object.DestroyImmediate (mf.sharedMesh);
				}
				mf.sharedMesh = null;
				//4 point .  position.     first 0,0     second width,0	 	third width,height	fourth 0,height	
				//first

				Vector3 first = new Vector3 (PositionX, PositionY, PositionZ);
				Vector3 second = new Vector3 (PositionX + Width, PositionY, PositionZ);
				Vector3 third = new Vector3 (PositionX + Width, PositionY - Height, PositionZ);
				Vector3 fourth = new Vector3 (PositionX, PositionY - Height, PositionZ);
				
				Mesh mesh = new Mesh ();
				object[] objArray1 = new object[] { "GeneratedMesh_", Width, "x", Height };
				mesh.name = string.Concat (objArray1);
				if (!Application.isPlaying) {
						mesh.hideFlags = HideFlags.DontSave;
				}

				mesh.vertices = new Vector3[] {first,second,third,fourth};
				mesh.triangles = new int[] { 0, 1, 2, 2, 3, 0 };
				
				SetUV (mesh);
				mf.sharedMesh = mesh;
				mf.sharedMesh.RecalculateNormals ();
				mf.sharedMesh.RecalculateBounds ();			
		}

		void SetUV (Mesh mesh)
		{
				float uv_x = 0;
				float uv_y = 0;
				float uv_width = 1;
				float uv_height = 1f;
				Vector2[] vectorArray = new Vector2[4];
				vectorArray [0].x = uv_x;
				vectorArray [0].y = uv_y + uv_height;
				vectorArray [1].x = uv_x + uv_width;
				vectorArray [1].y = uv_y + uv_height;
				vectorArray [2].x = uv_x + uv_width;
				vectorArray [2].y = uv_y;
				vectorArray [3].x = uv_x;
				vectorArray [3].y = uv_y;
				mesh.uv = vectorArray;
		}
		
		void OnResize ()
		{
				//mPX_Offset = (ParentWidth - Width) / 2;
		}

		void Update ()
		{
				OnResize ();
				RebuildMesh ();

				
		}
}
