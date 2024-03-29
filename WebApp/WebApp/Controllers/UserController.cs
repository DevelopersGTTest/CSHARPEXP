﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebApp.Models;
using WebApp.Models.ViewModels;

namespace WebApp.Controllers
{
    public class UserController : Controller
    {
        // GET: User
        public ActionResult Index()
        {
            List<ListUserViewModel> userList;
            using (PARROQUIAEntities db = new PARROQUIAEntities())
            {

                USUARIO userSession = (USUARIO)Session["User"];
                if (userSession != null)
                {
                    Session["UserId"] = userSession.id_usuario;
                    ViewBag.Message = "Bienvenido: " + Convert.ToString(userSession.username);
                }

                userList  = (from u in db.USUARIOs
                                select new ListUserViewModel
                                {
                                    id_user = u.id_usuario,
                                    id_rol = u.id_rol,
                                    nombre = u.nombre,
                                    apellido = u.apellido,
                                    fecha_creacion = u.fecha_creacion,
                                    username = u.username,
                                    password = u.password
                                }).ToList();

            }
            return View(userList);
        }

        public ActionResult BackPanel() {
            return RedirectToAction("Index", "Panel");
        }

        public ActionResult NewUser() {
            using (PARROQUIAEntities db = new PARROQUIAEntities())
            {
                USUARIO userSession = (USUARIO)Session["User"];
                if (userSession != null)
                {
                    ViewBag.Message = "Bienvenido: " + Convert.ToString(userSession.username);
                }
            }
                return View();
        }

        [HttpPost]
        public ActionResult SaveUser(string name, string lastname, string username, string password, string rol ) {
            using (PARROQUIAEntities db = new PARROQUIAEntities()) {
          
                USUARIO newUser = new USUARIO();
                newUser.id_rol = int.Parse(rol);
                newUser.nombre = name;
                newUser.apellido = lastname;
                newUser.fecha_creacion = DateTime.Parse("2021-11-24 12:35:29.123");
                newUser.username = username;
                newUser.password = password;

                db.USUARIOs.Add(newUser);
                db.SaveChanges();
            }
            return Redirect("Index");

        }

        [HttpGet]
        public ActionResult EditUser(int id_user) {
            UserViewModel model = new UserViewModel();
            using (PARROQUIAEntities db = new PARROQUIAEntities())
            {

                USUARIO userSession = (USUARIO)Session["User"];
                if (userSession != null)
                {
                    ViewBag.Message = "Bienvenido: " + Convert.ToString(userSession.username);
                }


                USUARIO userDb = db.USUARIOs.Find(id_user);
                model.id_user = userDb.id_usuario;
                model.nombre = userDb.nombre;
                model.apellido = userDb.apellido;
                model.username = userDb.username;
                model.password = userDb.password;
                model.id_rol = userDb.id_rol;
            }
                return View(model);
        }

        [HttpPost]
        public ActionResult ApplyEditUser(string name, string lastname, string username, string password, string rol, string id_user)
        {
            try
            {
                using (PARROQUIAEntities db = new PARROQUIAEntities())
                {

                    USUARIO userDb = db.USUARIOs.Find(int.Parse(id_user));
                    userDb.id_rol = int.Parse(rol);
                    userDb.nombre = name;
                    userDb.apellido = lastname;
                    userDb.fecha_creacion = DateTime.Parse("2021-11-24 12:35:29.123");
                    userDb.username = username;
                    userDb.password = password;

                    db.USUARIOs.Attach(userDb);
                    db.Entry(userDb).State = System.Data.Entity.EntityState.Modified;
                    db.SaveChanges();
                }

            } catch (Exception ex) {

            }
            return Redirect("Index");

        }


        [HttpGet]
        public ActionResult DeleteUser(int id_user) {
            using (PARROQUIAEntities db = new PARROQUIAEntities())
            {
                var userDb = db.USUARIOs.Find(id_user);
                db.USUARIOs.Remove(userDb);
                db.SaveChanges();
            }
            return Redirect("Index");
        }

    }
}