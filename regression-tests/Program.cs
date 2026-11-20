using Penumbra.GameData.Enums;
using System.Reflection;
void Check(bool ok,string name){if(!ok)throw new Exception(name);Console.WriteLine("PASS: "+name);}
var category=(EquipSlot)23;
Check(category.ToSlot()==EquipSlot.Body,"Ramza coat inventory slot is Body");
Check(category.ToHumanSlot()==HumanSlot.Body,"Ramza coat model slot is Body");
Check(category.IsEquipmentPiece(),"Ramza coat participates in identification");
var type=typeof(FullEquipTypeExtensions).GetMethod("ToEquipType",BindingFlags.Static|BindingFlags.Public,null,new[]{typeof(EquipSlot),typeof(WeaponCategory),typeof(bool)},null)!;
Check((FullEquipType)type.Invoke(null,new object[]{category,WeaponCategory.Unknown,true})! == FullEquipType.Body,"Ramza coat is included in Body item dictionary");
Check((int)EquipSlot.Nothing!=23,"synthetic Nothing does not mask real category 23");
Check(Penumbra.GameData.DataContainers.Version.ItemsByTypeOffset==3,"shared item cache version is invalidated");
