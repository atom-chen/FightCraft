--װ��ǿ���ȼ�
x100005_g_scriptId = 100005

--�¼�����
function x100005_OnEventFilter(uuid, achEventId, enchanceLevel)

	if enchanceLevel ~= nil and 5 <= LuaGetAllEquipsByEnchanceLevel(uuid, enchanceLevel) then
  		return 1
  	else
  		return 0
  	end
  	
end

--�ɾ���ɽ���
function x100005_OnAchievementProgress(uuid, reachNum)
  	return 1
end

