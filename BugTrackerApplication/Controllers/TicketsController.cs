﻿using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using BugTrackerApplication.Models;
using Microsoft.AspNet.Identity;

namespace BugTrackerApplication.Controllers
{
    public class TicketsController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: Tickets
        public ActionResult Index()
        {
            var tickets = db.Tickets.Include(t => t.Assignee).Include(t => t.Creator).Include(t => t.TicketPriority).Include(t => t.TicketStatus).Include(t => t.TicketType);
            return View(db.Tickets.ToList());
        }

        // GET: Tickets/Details/5
        [Authorize]
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Ticket ticket = db.Tickets.Find(id);
            if (ticket == null)
            {
                return HttpNotFound();
            }
            return View(ticket);
        }

        // GET: Tickets/Create
        [Authorize]
        public ActionResult Create()
        {
            ViewBag.ProjectId = new SelectList(db.Projects, "Id", "Name");
            ViewBag.AssigneeId = new SelectList(db.Users, "Id", "Name");
            ViewBag.CreatorId = new SelectList(db.Users, "Id", "Name");
            ViewBag.TicketPriorityId = new SelectList(db.TicketPriorities, "Id", "Title");
            ViewBag.TicketStatusId = new SelectList(db.TicketStatuses, "Id", "Title");
            ViewBag.TicketTypeId = new SelectList(db.TicketTypes, "Id", "Title");
            ViewBag.Created = DateTime.Now;
            return View();
        }

        // POST: Tickets/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize]
        public ActionResult Create([Bind(Include = "Id,Title,Description,TicketTypeId,TicketPriorityId,TicketStatusId,CreatorId,AssigneeId,ProjectId")] Ticket ticket)
        {
            if (ModelState.IsValid)
            {
                ticket.Created = DateTime.Now;
                ticket.TicketStatusId = 1;
                ticket.CreatorId = User.Identity.GetUserId();
                ticket.ProjectId = 1;
                db.Tickets.Add(ticket);
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.ProjectId = new SelectList(db.Projects, "Id", "Name");
            ViewBag.AssigneeId = new SelectList(db.Users, "Id", "Name", ticket.AssigneeId);
            ViewBag.CreatorId = new SelectList(db.Users, "Id", "Name", ticket.CreatorId);
            ViewBag.TicketPriorityId = new SelectList(db.TicketPriorities, "Id", "Title", ticket.TicketPriorityId);
            ViewBag.TicketStatusId = new SelectList(db.TicketStatuses, "Id", "Title", ticket.TicketStatusId);
            ViewBag.TicketTypeId = new SelectList(db.TicketTypes, "Id", "Title", ticket.TicketTypeId);
            ViewBag.Created = DateTime.Now;
            return View(ticket);
        }

        // GET: Tickets/Edit/5
        [Authorize]
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Ticket ticket = db.Tickets.Find(id);
            if (ticket == null)
            {
                return HttpNotFound();
            }
            ViewBag.ProjectId = new SelectList(db.Projects, "Id", "Name");
            ViewBag.AssigneeId = new SelectList(db.Users, "Id", "Name", ticket.AssigneeId);
            ViewBag.CreatorId = new SelectList(db.Users, "Id", "Name", ticket.CreatorId);
            ViewBag.TicketPriorityId = new SelectList(db.TicketPriorities, "Id", "Title", ticket.TicketPriorityId);
            ViewBag.TicketStatusId = new SelectList(db.TicketStatuses, "Id", "Title", ticket.TicketStatusId);
            ViewBag.TicketTypeId = new SelectList(db.TicketTypes, "Id", "Title", ticket.TicketTypeId);
            return View(ticket);
        }

        // POST: Tickets/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize]
        public ActionResult Edit([Bind(Include = "Id,Title,Description,TicketTypeId,TicketPriorityId,CreatorId,AssigneeId,ProjectId")] Ticket ticket)
        {
            if (ModelState.IsValid)
            {
                //ticket.Updated = DateTime.Now;
                //db.Entry(ticket).State = EntityState.Modified;
                //db.SaveChanges();
                //return RedirectToAction("Index");
                var ticketDb = db.Tickets.Where(p => p.Id == ticket.Id).FirstOrDefault();

                ticketDb.Updated = DateTime.Now;
                ticketDb.Title = ticket.Title;
                ticketDb.Description = ticket.Description;

                db.SaveChanges();
                return RedirectToAction("Index", "Tickets");
            }
            
                
                    
                

               
            
            ViewBag.AssigneeId = new SelectList(db.Users, "Id", "Name", ticket.AssigneeId);
            ViewBag.CreatorId = new SelectList(db.Users, "Id", "Name", ticket.CreatorId);
            ViewBag.TicketPriorityId = new SelectList(db.TicketPriorities, "Id", "Title", ticket.TicketPriorityId);
            ViewBag.TicketStatusId = new SelectList(db.TicketStatuses, "Id", "Title", ticket.TicketStatusId);
            ViewBag.TicketTypeId = new SelectList(db.TicketTypes, "Id", "Title", ticket.TicketTypeId);
            ViewBag.ProjectId = new SelectList(db.Projects, "Id", "Name");
            return View(ticket);
        }

        // GET: Tickets/Delete/5
        [Authorize]
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Ticket ticket = db.Tickets.Find(id);
            if (ticket == null)
            {
                return HttpNotFound();
            }
            return View(ticket);
        }

        // POST: Tickets/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Ticket ticket = db.Tickets.Find(id);
            db.Tickets.Remove(ticket);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        [Authorize(Roles = "Submitter")]
        public ActionResult ShowSubmitterTickets()
        {
            var userId = User.Identity.GetUserId();
            var tickets = db.Tickets.Where(t => t.CreatorId == userId).Include(t => t.Assignee).Include(t => t.Creator).Include(t => t.TicketPriority).Include(t => t.TicketStatus).Include(t => t.TicketType);
            return View("Index", db.Tickets.ToList());
        }

        [Authorize(Roles = "Developer")]
        public ActionResult ShowDeveloperTickets()
        {
            var userId = User.Identity.GetUserId();
            var tickets = db.Tickets.Where(t => t.AssigneeId == userId).Include(t => t.Assignee).Include(t => t.Creator).Include(t => t.TicketPriority).Include(t => t.TicketStatus).Include(t => t.TicketType);
            return View("Index", db.Tickets.ToList());
        }

    }
}
