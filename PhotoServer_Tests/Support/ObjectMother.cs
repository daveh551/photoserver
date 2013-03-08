using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PhotoServer.Domain;

namespace PhotoServer_Tests.Support
{
    public static class ObjectMother
    {
		public static void ClearDirectory()
		{
			var photoPath = @"..\..\..\PhotoServer\Photos\Test";
			var directoryInfo = new DirectoryInfo(photoPath);
			if (directoryInfo.Exists)
			{
				directoryInfo.Delete(true);
			}

		}

		private static PhotoServer.Domain.PhotoData[] testData =

	    new PhotoServer.Domain.PhotoData []
	    {
		    new PhotoData
			    {
				    Id = new Guid("E0CAF539-5C32-432B-AAC4-B01CD4EABB3A"),
				    Race = "Test",
				    Station = "FinishLine",
				    Card = "1",
				    Sequence = 1,
				    Path = @"Test/FinishLine/1/001.JPG",
				    Hres = 3008,
				    Vres = 2000,
				    TimeStamp = new DateTime(2011, 10, 22, 8, 48, 59, 60)
			    },
		    new PhotoData
			    {
				    Id = new Guid("24249DF5-C9FF-4B17-A259-514586F07C6B"),
				    Race = "Test",
				    Station = "FinishLine",
				    Card = "1",
				    Sequence = 3,
				    Path = @"Test/FinishLine/1/003.JPG",
				    Hres = 3008,
				    Vres = 2000,
				    TimeStamp = new DateTime(2011, 10, 22, 8, 29, 0)
			    },
		    new PhotoData
			    {
				    Id = new Guid("E5034D6B-3B26-4F03-8F9E-0EA1F5BE55C7"),
				    Race = "Test",
				    Station = "FinishLine",
				    Card = "1",
				    Sequence = 4,
				    Path = @"Test/FinishLine/1/004.JPG",
				    Hres = 3008,
				    Vres = 2000,
				    TimeStamp = new DateTime(2011, 10, 22, 8, 29, 0)
			    }
	    };


	    public static List<PhotoServer.Domain.PhotoData> ReturnPhotoDataRecord(int count)
	    {
		    var returnList = new List<PhotoData>();
		    if (count <= 3)
				for (int ix = 0; ix < count; ix++)
				{
					returnList.Add(testData[ix]);
				}

		    return returnList;

	    }

	    public static void CopyTestFiles()
	    {
		    throw new NotImplementedException();
	    }
    }
}
