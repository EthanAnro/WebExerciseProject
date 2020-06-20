using System.Collections.Generic;
using System.Linq;
using ContosoUniversity.Data;
using ContosoUniversity.Models;
using ContosoUniversity.Models.SchoolViewModels;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace ContosoUniversity.Pages.Instructors {
    public class InstructorCoursesPageModel : PageModel {
        public List<AssignedCourseData> AssignedCourseDataList;
        public void PopulateAssignedCourseData (SchoolContext context, Instructor instructor) {
            var allCourses = context.Courses;
            var InstructorCourses = new HashSet<int> (instructor.CourseAssignments.Select (c => c.CourseID));
            AssignedCourseDataList = new List<AssignedCourseData> ();
            foreach (var course in allCourses) {
                AssignedCourseDataList.Add (new AssignedCourseData {
                    CourseID = course.CourseID,
                        Title = course.Title,
                        Assigned = InstructorCourses.Contains (course.CourseID)
                });
            }
        }

        public void UpdateInStructorCourses (SchoolContext context, string[] selectedCourse, Instructor instrutorToUpdate) {
            if (selectedCourse == null) {
                instrutorToUpdate.CourseAssignments = new List<CourseAssignment> ();
                return;
            }

            var selectedCoursesHS=new HashSet<string>(selectedCourse);
            var InstructorCourses =new HashSet<int>();
        }
    }
}