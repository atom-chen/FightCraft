--��ɫװ������
x100011_g_scriptId = 100011


--�¼�����
function x100011_OnEventFilter(uuid, achEventId, count)

	if count ~= nil and tonumber(count) <= LuaGetAllEquipsByQuality(uuid, 1) then
  		return 1
  	else
  		return 0
  	end
  	
end

--�ɾ���ɽ���
function x100011_OnAchievementProgress(uuid, reachNum)
  	return 1
end

