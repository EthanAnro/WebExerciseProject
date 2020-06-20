using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ContosoUniversity.Data;
using ContosoUniversity.Models;
using ContosoUniversity.Models.SchoolViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

namespace ContosoUniversity.Pages.Instructors {
    public class IndexModel : PageModel {
        private readonly ContosoUniversity.Data.SchoolContext _context;

        public IndexModel (ContosoUniversity.Data.SchoolContext context) {
            _context = context;
        }
        public InstructorIndexData InstructorData { get; set; }
        public int InstructorID { get; set; }
        public int CourseID { get; set; }
        public IList<Instructor> Instructor { get; set; }

        public async Task OnGetAsync (int? id, int? courseID) {
            InstructorData = new InstructorIndexData ();
            InstructorData.Instructors = await _context.Instructors
                .Include (oa => oa.OfficeAssignment)
                
                .Include (ca => ca.CourseAssignments)
                    .ThenInclude (c => c.Course)
                        .ThenInclude (d => d.Department)
                //***
                // .Include (ca => ca.CourseAssignments)
                //     .ThenInclude (c => c.Course).Where ()
                //         .ThenInclude (e => e.Enrollments)
                //             .ThenInclude (s => s.Student)
                // .AsNoTracking ()
                ///***
                .OrderBy (ln => ln.LastName)
                .ToListAsync ();

            if (id != null) {
                InstructorID = id.Value;
                Instructor instructor = InstructorData.Instructors
                    .Where (i => i.ID == id.Value).SingleOrDefault ();
                InstructorData.Courses = instructor.CourseAssignments.Select (c => c.Course);
            }

            if (courseID != null) {
                CourseID = courseID.Value;
                var selectedCourse = InstructorData.Courses
                    .Where (x => x.CourseID == courseID).SingleOrDefault ();
                    await _context.Entry(selectedCourse).Collection(x=>x.Enrollments).LoadAsync();
                    foreach (Enrollment enrollment in selectedCourse.Enrollments)
                    {
                        await _context.Entry(enrollment).Reference(x=>x.Student).LoadAsync();
                    }
                InstructorData.Enrollments = selectedCourse.Enrollments;
            }
        }
    }
}