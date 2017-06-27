using System;
using System.Collections.Generic;

namespace AudioKetab
{
	public class CarouselDataSourceModel
	{
		
			public List<Reading_mentorModel> list_Reading_Mentor { get; set; }
            public List<Book_summariesModel> list_Book_Summeries { get; set; }
		    public List<LectureandTraingingModel> list_LectureandTraining { get; set; }
		    public List<NewsLetterModel> list_NewsLetter { get; set; }
		    public double cell_height { get; set; }

	}
}
